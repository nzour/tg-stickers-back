using System;
using System.Collections.Generic;

namespace TgStickers.Application.StickerPacks
{
    public class StickerPackInput
    {
        public string Name { get; set; } = string.Empty;

        public string? Alias { get; set; }

        public IEnumerable<Guid> TagIds { get; set; } = new List<Guid>();
    }
}