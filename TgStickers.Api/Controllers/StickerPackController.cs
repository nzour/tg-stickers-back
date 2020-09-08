using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TgStickers.Api.Services;
using TgStickers.Application.StickerPacks;
using TgStickers.Application.StickerPacks.Filters;
using TgStickers.Infrastructure.Paginating;
using TgStickers.Infrastructure.Telegram;
using TgStickers.Infrastructure.Transaction;

namespace TgStickers.Api.Controllers
{
    [Route("stickers")]
    public class StickerPackController : Controller
    {
        private readonly StickerPackService _stickerPackService;
        private readonly CurrentAdminProvider _currentAdminProvider;
        private readonly ITransactional _transactional;
        private readonly TelegramBot _tgBot;

        public StickerPackController(
            StickerPackService stickerPackService,
            CurrentAdminProvider currentAdminProvider,
            ITransactional transactional,
            TelegramBot tgBot
        )
        {
            _stickerPackService = stickerPackService;
            _currentAdminProvider = currentAdminProvider;
            _transactional = transactional;
            _tgBot = tgBot;
        }

        /// <summary>
        /// Получение всех стикерпаков
        /// </summary>
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

        /// <summary>
        /// Добавление стикерпака 
        /// </summary>
        [HttpPost]
        public async Task<StickerPackOutput> CreateStickerPackAsync([FromBody] StickerPackInput input)
        {
            var currentAdmin = await _currentAdminProvider.ProviderCurrentAdminAsync();

            return await _transactional.ExecuteAsync(async () =>
                await _stickerPackService.CreateStickerPackAsync(currentAdmin, input));
        }

        /// <summary>
        /// Обновление стикерпака
        /// </summary>
        [HttpPut("{stickerPackId:guid}")]
        public async Task<StickerPackOutput> UpdateStickerPackAsync([FromRoute] Guid stickerPackId, [FromBody] StickerPackInput input)
        {
            var currentAdmin = await _currentAdminProvider.ProviderCurrentAdminAsync();

            return await _transactional.ExecuteAsync(async () =>
                await _stickerPackService.UpdateStickerPackAsync(currentAdmin, stickerPackId, input));
        }

        /// <summary>
        /// Удаление стикер пака
        /// </summary>
        [HttpDelete("{stickerPackId:guid}")]
        public async Task RemoveStickerPackAsync([FromRoute] Guid stickerPackId)
        {
            var currentAdmin = await _currentAdminProvider.ProviderCurrentAdminAsync();

            await _transactional.ExecuteAsync(async () =>
                await _stickerPackService.RemoveStickerPackAsync(currentAdmin, stickerPackId));
        }

        /// <summary>
        /// Получение стикер пака по его идентификатору
        /// </summary>
        [HttpGet("{stickerPackId:guid}"), AllowAnonymous]
        public async Task<StickerPackOutput> GetStickerPackByIdAsync([FromRoute] Guid stickerPackId)
        {
            var stickerPack = await _stickerPackService.GetStickerPackAsync(stickerPackId);
            var stickers = await _tgBot.GetStickerFilesFromPackAsync(stickerPack.Name);
            var filePath = await _tgBot.GetFilePathAsync(stickerPack.Name, fileId: stickers.First());

            return new StickerPackOutput(stickerPack, firstStickerPath: filePath, stickersCount: stickers.Count());
        }

        /// <summary>
        /// Batch увеличение рейтингов стикер паков
        /// </summary>
        [HttpPatch("claps"), AllowAnonymous]
        public async Task IncreaseClapsAsync([FromBody] IncreaseClapsInput input)
        {
            await _transactional.ExecuteAsync(async () =>
                await _stickerPackService.IncreaseClapsAsync(input.ClapsInput));
        }

        /// <summary>
        /// Проверка существует ли названия стикер пак с название
        /// </summary>
        [HttpHead("{name}/exists")]
        public async Task<StatusCodeResult> IsStickerPackExistsWithNameAsync([FromRoute] string name)
        {
            return await _tgBot.IsStickerPackExistsAsync(name)
                ? new OkResult() as StatusCodeResult
                : new NotFoundResult();
        }

        /// <summary>
        /// Получение изображений стикер паков
        /// </summary>
        [HttpGet("{stickerPackId:guid}/images"), AllowAnonymous]
        public async Task<IEnumerable<string>> GetStickerPackImagesAsync([FromRoute] Guid stickerPackId)
        {
            return await _stickerPackService.GetStickerPackImages(stickerPackId);
        }

        /// <summary>
        /// Отправка ссылки в телеграм через бота
        /// </summary>
        [HttpPost("send"), AllowAnonymous]
        public async Task SendStickerPackToTelegram([FromBody] SendStickerPackToTelegramInput input)
        {
            await _tgBot.SendMessageAsync(receiver: input.TgUsername.TrimStart('@'), input.StickerPackName);
        }
    }
}
