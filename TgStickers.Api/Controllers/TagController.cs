using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TgStickers.Application.Tags;
using TgStickers.Infrastructure.Paginating;
using TgStickers.Infrastructure.Transaction;

namespace TgStickers.Api.Controllers
{
    [Microsoft.AspNetCore.Components.Route("tags")]
    public class TagController : Controller
    {
        private readonly ITransactional _transactional;
        private readonly TagService _tagService;

        public TagController(ITransactional transactional, TagService tagService)
        {
            _transactional = transactional;
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<PaginatedData<TagOutput>> GetAllTagsAsync(
            [FromQuery] Pagination pagination,
            [FromQuery] TagNameFilter tagNameFilter
        )
        {
            return await _tagService.FindAllTagsAsync(tagNameFilter, pagination);
        }

        [HttpPost, Authorize]
        public async Task<TagOutput> CreateTagAsync([FromBody] TagInput input)
        {
            return await _transactional.ExecuteAsync(async () => await _tagService.CreateTagAsync(input));
        }

        [HttpPut("{tagId:guid}"), Authorize]
        public async Task<TagOutput> UpdateTagAsync([FromRoute] Guid tagId, [FromBody] TagInput input)
        {
            return await _transactional.ExecuteAsync(async () => await _tagService.UpdateTagAsync(tagId, input));
        }
    }
}