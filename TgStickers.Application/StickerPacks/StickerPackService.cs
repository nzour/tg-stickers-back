using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Conventions;
using NHibernate.Linq;
using TgStickers.Application.Common;
using TgStickers.Application.Exceptions;
using TgStickers.Application.StickerPacks.Filters;
using TgStickers.Domain;
using TgStickers.Domain.Entity;
using TgStickers.Infrastructure.Paginating;
using TgStickers.Infrastructure.Telegram;
using static TgStickers.Application.Common.SearchType;
using static TgStickers.Application.Common.SortType;
using static TgStickers.Application.StickerPacks.Filters.StickerPackSortingField;

namespace TgStickers.Application.StickerPacks
{
    public class StickerPackService
    {
        private readonly IRepository<StickerPack> _stickerPackRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly TelegramBot _tgBot;

        public StickerPackService(IRepository<StickerPack> stickerPackRepository, IRepository<Tag> tagRepository, TelegramBot tgBot)
        {
            _stickerPackRepository = stickerPackRepository;
            _tagRepository = tagRepository;
            _tgBot = tgBot;
        }

        public async Task<PaginatedData<StickerPackOutput>> FindAllStickerPacksAsync(
            StickerPackSorting sorting,
            StickerPackNameFilter nameFilter,
            StickerPackClapsFilter clapsFilter,
            StickerPackTagsFilter tagsFilter,
            Pagination pagination
        )
        {
            var stickerPacks = _stickerPackRepository.FindAll();

            stickerPacks = ApplySorting(stickerPacks, sorting.SortBy, sorting.SortType);
            stickerPacks = ApplyNameFilter(stickerPacks, nameFilter);
            stickerPacks = ApplyClapsFilter(stickerPacks, clapsFilter);
            stickerPacks = await ApplyTagsFilterAsync(stickerPacks, tagsFilter);

            var paginatedStickers = await stickerPacks.PaginateAsync(pagination);

            var result = new PaginatedData<StickerPackOutput>(paginatedStickers.Total, new List<StickerPackOutput>());

            foreach (var stickerPack in paginatedStickers.Data)
            {
                var stickers = await _tgBot.GetStickerFilesFromPackAsync(stickerPack.Name);
                var filePath = await _tgBot.GetFileFullPathAsync(stickerPack.Name, fileId: stickers.First());

                result.Data = result.Data.Append(new StickerPackOutput(stickerPack, filePath, stickers.Count()));
            }

            return result;
        }

        public async Task<StickerPackOutput> CreateStickerPackAsync(Admin currentAdmin, StickerPackInput input)
        {
            if (!await _tgBot.IsStickerPackExistsAsync(input.Name))
            {
                throw StickerPackException.StickerPackDoesNotExists(input.Name);
            }

            var stickerPack = currentAdmin.AddNewStickerPack(input.Name, input.SharedUrl, await FindTagsAsync(input.TagIds));

            var stickers = await _tgBot.GetStickerFilesFromPackAsync(stickerPack.Name);
            var filePath = await _tgBot.GetFileFullPathAsync(stickerPack.Name, fileId: stickers.First());

            return new StickerPackOutput(stickerPack, filePath, stickers.Count());
        }

        public async Task<StickerPackOutput> UpdateStickerPackAsync(Admin currentAdmin, Guid stickerPackId, StickerPackInput input)
        {
            var stickerPack = await GetStickerPack(stickerPackId);

            StickerPackException.AssertOwnStickerPack(currentAdmin, stickerPack);

            if (!await _tgBot.IsStickerPackExistsAsync(input.Name))
            {
                throw StickerPackException.StickerPackDoesNotExists(input.Name);
            }

            stickerPack.Name = input.Name;
            stickerPack.SharedUrl = input.SharedUrl;
            stickerPack.ReplaceTags(await FindTagsAsync(input.TagIds));

            var stickers = await _tgBot.GetStickerFilesFromPackAsync(stickerPack.Name);
            var filePath = await _tgBot.GetFileFullPathAsync(stickerPack.Name, fileId: stickers.First());

            return new StickerPackOutput(stickerPack, filePath, stickers.Count());
        }

        public async Task RemoveStickerPackAsync(Admin currentAdmin, Guid stickerPackId)
        {
            var stickerPack = await GetStickerPack(stickerPackId);

            StickerPackException.AssertOwnStickerPack(currentAdmin, stickerPack);

            await _stickerPackRepository.RemoveAsync(stickerPack);
        }

        public async Task IncreaseClapsAsync(IEnumerable<ClapsToAddInput> inputs)
        {
            foreach (var input in inputs)
            {
                var stickerPack = await _stickerPackRepository.FindByIdAsync(input.StickerPackId);

                stickerPack?.IncreaseClaps((int) input.ClapsToAdd);
            }
        }

        private async Task<StickerPack> GetStickerPack(Guid stickerPackId)
        {
            var stickerPack = await _stickerPackRepository.FindByIdAsync(stickerPackId);

            if (null == stickerPack)
            {
                throw NotFoundException<StickerPack>.WithId(stickerPackId);
            }

            return stickerPack;
        }

        private async Task<IEnumerable<Tag>> FindTagsAsync(IEnumerable<Guid> tagIds)
        {
            return tagIds.IsNotEmpty()
                ? await _tagRepository.FindAll().Where(tag => tagIds.Contains(tag.Id)).ToListAsync()
                : Enumerable.Empty<Tag>();
        }

        private static IQueryable<StickerPack> ApplySorting(
            IQueryable<StickerPack> stickerPacks,
            StickerPackSortingField sortingField,
            SortType sortType
        )
        {
            return sortingField switch
            {
                CreatedAt when sortType == Descending => stickerPacks.OrderByDescending(s => s.CreatedAt),
                CreatedAt when sortType == Ascending => stickerPacks.OrderBy(s => s.CreatedBy),

                DonationCount when sortType == Descending => stickerPacks.OrderByDescending(s => s.Donations.Count),
                DonationCount when sortType == Ascending => stickerPacks.OrderBy(s => s.Donations.Count),

                ClapCount when sortType == Descending => stickerPacks.OrderByDescending(s => s.Claps),
                ClapCount when sortType == Ascending => stickerPacks.OrderBy(s => s.Claps),

                _ => throw new ArgumentOutOfRangeException(nameof(sortingField), sortingField, $"Sorting by field '{sortingField}' not implemented")
            };
        }

        private static IQueryable<StickerPack> ApplyNameFilter(IQueryable<StickerPack> stickerPacks, StickerPackNameFilter filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Name))
            {
                return stickerPacks;
            }

            return filter.NameSearchType switch
            {
                Contains => stickerPacks.Where(s => s.Name.Contains(filter.Name)),
                SearchType.Equals => stickerPacks.Where(s => filter.Name == s.Name),
                _ => stickerPacks
            };
        }

        private static IQueryable<StickerPack> ApplyClapsFilter(IQueryable<StickerPack> stickerPacks, StickerPackClapsFilter filter)
        {
            if (null == filter.ClapsCount)
            {
                return stickerPacks;
            }

            return filter.ClapsSearchType switch
            {
                SearchType.Equals => stickerPacks.Where(s => filter.ClapsCount == s.Claps),
                GreaterThan => stickerPacks.Where(s => s.Claps > filter.ClapsCount),
                GreaterOrEquals => stickerPacks.Where(s => s.Claps >= filter.ClapsCount),
                LessThan => stickerPacks.Where(s => s.Claps < filter.ClapsCount),
                LessOrEquals => stickerPacks.Where(s => s.Claps <= filter.ClapsCount),
                _ => stickerPacks
            };
        }

        private async Task<IQueryable<StickerPack>> ApplyTagsFilterAsync(IQueryable<StickerPack> stickerPacks, StickerPackTagsFilter filter)
        {
            if (filter.TagIds.IsEmpty())
            {
                return stickerPacks;
            }

            var tagsIds = await _tagRepository.FindAll()
                .Where(tag => filter.TagIds.Contains(tag.Id))
                .Select(tag => tag.Id)
                .ToListAsync();

            return stickerPacks.Where(stickerPack =>
                stickerPack.Tags.Any(t => tagsIds.Contains(t.Id)));
        }
    }
}
