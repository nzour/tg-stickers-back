using System;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Linq;
using TgStickers.Application.Common;
using TgStickers.Application.Exceptions;
using TgStickers.Domain;
using TgStickers.Domain.Entity;
using TgStickers.Infrastructure.Paginating;

namespace TgStickers.Application.Tags
{
    public class TagService
    {
        private readonly IRepository<Tag> _tagRepository;

        public TagService(IRepository<Tag> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<PaginatedData<TagOutput>> FindAllTagsAsync(TagNameFilter tagNameFilter, Pagination pagination)
        {
            var tags = _tagRepository.FindAll();

            if (tagNameFilter.HasValue)
            {
                tags = tagNameFilter.SearchType == SearchType.Equals
                    ? tags.Where(tag => tagNameFilter.Name == tag.Name)
                    : tags.Where(tag => tag.Name.Contains(tagNameFilter.Name));
            }

            return await tags
                .Select(tag => new TagOutput(tag))
                .PaginateAsync(pagination);
        }

        public async Task<TagOutput> CreateTagAsync(TagInput input)
        {
            if (await IsTagNameBusyAsync(input.Name))
            {
                throw TagException.NameIsBusy(input.Name);
            }

            var tag = new Tag(input.Name);

            await _tagRepository.SaveAsync(tag);

            return new TagOutput(tag);
        }

        public async Task<TagOutput> UpdateTagAsync(Guid tagId, TagInput input)
        {
            var tag = await _tagRepository.FindByIdAsync(tagId);

            if (null == tag)
            {
                throw NotFoundException<Tag>.WithId(tagId);
            }

            if (await IsTagNameBusyAsync(input.Name))
            {
                throw TagException.NameIsBusy(input.Name);
            }

            tag.Name = input.Name;

            return new TagOutput(tag);
        }

        public async Task<bool> IsTagNameBusyAsync(string name)
        {
            return 0 != await _tagRepository.FindAll()
                .Where(tag => name == tag.Name)
                .CountAsync();
        }
    }
}
