using System;
using TgStickers.Application.Exceptions;

namespace TgStickers.Application.StickerPacks
{
    public class UpdateStickerPackException : AbstractHandledException
    {
        public UpdateStickerPackException(string message) : base(message)
        {
        }

        public static UpdateStickerPackException StickerPackDoesNotBelongToYou(Guid stickerPackId)
        {
            return new UpdateStickerPackException($"Sticker pack '{stickerPackId}' does not belong to you!");
        }
    }
}