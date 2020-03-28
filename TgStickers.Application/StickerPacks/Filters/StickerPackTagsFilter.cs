using System;
using System.Collections.Generic;

namespace TgStickers.Application.StickerPacks.Filters
{
    public class StickerPackTagsFilter
    {
        public IEnumerable<Guid> TagIds { get; set; } = new List<Guid>();
    }
}