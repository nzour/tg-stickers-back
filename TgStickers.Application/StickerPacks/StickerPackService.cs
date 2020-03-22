using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Conventions;
using FluentNHibernate.Utils;
using NHibernate.Linq;
using TgStickers.Application.Common;
using TgStickers.Application.Exceptions;
using TgStickers.Application.StickerPacks.Filters;
using TgStickers.Domain;
using TgStickers.Domain.Entity;
using TgStickers.Infrastructure.Paginating;
using static TgStickers.Application.Common.SearchType;
using static TgStickers.Application.Common.SortType;
using static TgStickers.Application.StickerPacks.Filters.StickerPackSortingField;

namespace TgStickers.Application.StickerPacks
{
    public class StickerPackService
    {
        private readonly IRepository<StickerPack> _stickerPackRepository;
        private readonly IRepository<Tag> _tagRepository;

        public StickerPackService(IRepository<StickerPack> stickerPackRepository, IRepository<Tag> tagRepository)
        {
            _stickerPackRepository = stickerPackRepository;
            _tagRepository = tagRepository;
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

            return await stickerPacks
                .Select(s => new StickerPackOutput(s))
                .PaginateAsync(pagination);
        }

        public async Task<StickerPackOutput> CreateStickerPackAsync(Admin currentAdmin, StickerPackInput input)
        {
            var stickerPack = currentAdmin.AddNewStickerPack(input.Name, input.SharedUrl, await FindTagsAsync(input.TagIds));

            return new StickerPackOutput(stickerPack);
        }

        public async Task<StickerPackOutput> UpdateStickerPackAsync(Admin currentAdmin, Guid stickerPackId, StickerPackInput input)
        {
            var stickerPack = await GetStickerPack(stickerPackId);

            if (!currentAdmin.IsOwnerOf(stickerPack))
            {
                throw UpdateStickerPackException.StickerPackDoesNotBelongToYou(stickerPackId);
            }

            stickerPack.Name = input.Name;
            stickerPack.SharedUrl = input.SharedUrl;
            stickerPack.ReplaceTags(await FindTagsAsync(input.TagIds));

            return new StickerPackOutput(stickerPack);
        }

        public async Task<StickerPackOutput> IncreaseClapsAsync(Guid stickerPackId, uint clapsToAdd)
        {
            var stickerPack = await GetStickerPack(stickerPackId);

            stickerPack.IncreaseClaps(clapsToAdd);

            return new StickerPackOutput(stickerPack);
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
            if (!filter.HasValue)
            {
                return stickerPacks;
            }

            return filter.SearchType switch
            {
                Contains => stickerPacks.Where(s => s.Name.Contains(filter.Name)),
                SearchType.Equals => stickerPacks.Where(s => filter.Name == s.Name),
                _ => stickerPacks
            };
        }

        private static IQueryable<StickerPack> ApplyClapsFilter(IQueryable<StickerPack> stickerPacks, StickerPackClapsFilter filter)
        {
            if (!filter.HasValue)
            {
                return stickerPacks;
            }

            return filter.SearchType switch
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
            if (!filter.HasValue)
            {
                return stickerPacks;
            }

            var tags = await _tagRepository.FindAll()
                .Where(tag => filter.TagIds.Contains(tag.Id))
                .ToListAsync();

            return stickerPacks.Where(stickerPack => tags.In(stickerPack.Tags));
        }
    }
}