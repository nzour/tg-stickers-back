using System;

namespace TgStickers.Application.StickerPacks
{
    public class IncreaseClapsInput
    {
        public Guid StickerPackId { get; set; }
        public uint ClapsToAdd { get; set; }
    }
}