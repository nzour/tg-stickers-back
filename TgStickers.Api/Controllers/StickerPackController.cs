using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TgStickers.Api.Services;
using TgStickers.Application.StickerPacks;
using TgStickers.Application.StickerPacks.Filters;
using TgStickers.Infrastructure.Paginating;
using TgStickers.Infrastructure.Transaction;

namespace TgStickers.Api.Controllers
{
    [Route("stickers")]
    public class StickerPackController : Controller
    {
        private readonly StickerPackService _stickerPackService;
        private readonly CurrentAdminProvider _currentAdminProvider;
        private readonly ITransactional _transactional;

        public StickerPackController(
            StickerPackService stickerPackService,
            CurrentAdminProvider currentAdminProvider,
            ITransactional transactional
        )
        {
            _stickerPackService = stickerPackService;
            _currentAdminProvider = currentAdminProvider;
            _transactional = transactional;
        }

        [HttpGet, AllowAnonymous]
        public async Task<PaginatedData<StickerPackOutput>> GetAllStickerPacksAsync(
            [FromQuery] StickerPackSorting sorting,
            [FromQuery] StickerPackNameFilter nameFilter,
            [FromQuery] StickerPackClapsFilter clapsFilter,
            [FromQuery] StickerPackTagsFilter tagsFilter,
            [FromQuery] Pagination pagination
        )
        {
            return await _stickerPackService.FindAllStickerPacksAsync(sorting, nameFilter, clapsFilter, tagsFilter, pagination);
        }

        [HttpPost]
        public async Task<StickerPackOutput> CreateStickerPackAsync([FromBody] StickerPackInput input)
        {
            var currentAdmin = await _currentAdminProvider.ProviderCurrentAdminAsync();

            return await _transactional.ExecuteAsync(async () =>
                await _stickerPackService.CreateStickerPackAsync(currentAdmin, input));
        }

        [HttpPut("{stickerPackId:guid}")]
        public async Task<StickerPackOutput> UpdateStickerPackAsync([FromRoute] Guid stickerPackId, [FromBody] StickerPackInput input)
        {
            var currentAdmin = await _currentAdminProvider.ProviderCurrentAdminAsync();

            return await _transactional.ExecuteAsync(async () =>
                await _stickerPackService.UpdateStickerPackAsync(currentAdmin, stickerPackId, input));
        }

        [HttpPatch("{stickerPackId:guid}/claps"), AllowAnonymous]
        public async Task<StickerPackOutput> IncreaseClapsAsync([FromRoute] Guid stickerPackId, [FromBody] IncreaseClapsInput input)
        {
            return await _transactional.ExecuteAsync(async () =>
                await _stickerPackService.IncreaseClapsAsync(stickerPackId, input.ClapsToAdd));
        }
    }
}