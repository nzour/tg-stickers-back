using System;
using TgStickers.Application.Exceptions;
using TgStickers.Domain.Entity;

namespace TgStickers.Application.StickerPacks
{
    public class StickerPackException : AbstractHandledException
    {
        public StickerPackException(string message) : base(message)
        {
        }

        public static StickerPackException StickerPackDoesNotBelongToYou(Guid stickerPackId)
        {
            return new StickerPackException($"Sticker pack '{stickerPackId}' does not belong to you!");
        }

        public static StickerPackException StickerPackDoesNotExists(string stickerPackName)
        {
            return new StickerPackException($"Sticker pack with name '{stickerPackName}' does not exists!");
        }

        public static void AssertOwnStickerPack(Admin admin, StickerPack stickerPack)
        {
            if (admin.IsOwnerOf(stickerPack))
            {
                return;
            }

            throw StickerPackDoesNotBelongToYou(stickerPack.Id);
        }
    }
}