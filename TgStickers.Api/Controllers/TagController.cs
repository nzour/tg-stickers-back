using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TgStickers.Application.Tags;
using TgStickers.Infrastructure.Paginating;
using TgStickers.Infrastructure.Transaction;

namespace TgStickers.Api.Controllers
{
    [Route("tags")]
    public class TagController : Controller
    {
        private readonly ITransactional _transactional;
        private readonly TagService _tagService;

        public TagController(ITransactional transactional, TagService tagService)
        {
            _transactional = transactional;
            _tagService = tagService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<PaginatedData<TagOutput>> GetAllTagsAsync(
            [FromQuery] Pagination pagination,
            [FromQuery] TagNameFilter tagNameFilter
        )
        {
            return await _tagService.FindAllTagsAsync(tagNameFilter, pagination);
        }

        [HttpPost]
        public async Task<TagOutput> CreateTagAsync([FromBody] TagInput input)
        {
            return await _transactional.ExecuteAsync(async () => await _tagService.CreateTagAsync(input));
        }

        [HttpPut("{tagId:guid}")]
        public async Task<TagOutput> UpdateTagAsync([FromRoute] Guid tagId, [FromBody] TagInput input)
        {
            return await _transactional.ExecuteAsync(async () => await _tagService.UpdateTagAsync(tagId, input));
        }

        [HttpHead("name-busy")]
        public async Task<StatusCodeResult> IsTagNameBusyAsync([FromBody] TagInput input)
        {
            return await _tagService.IsTagNameBusyAsync(input.Name)
                ? new StatusCodeResult(200)
                : new StatusCodeResult(404);
        }
    }
}
