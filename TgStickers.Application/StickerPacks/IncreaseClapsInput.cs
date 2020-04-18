using System;
using System.Collections.Generic;

namespace TgStickers.Application.StickerPacks
{
    public class IncreaseClapsInput
    {
        public IEnumerable<ClapsToAddInput> ClapsInput { get; set; } = new List<ClapsToAddInput>();
    }

    public class ClapsToAddInput
    {
        public Guid StickerPackId { get; set; }
        public uint ClapsToAdd { get; set; }
    }
}