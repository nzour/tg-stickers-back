using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TgStickers.Application.StickerPacks
{
    public class StickerPackInput
    {
        public string Name { get; set; } = string.Empty;

        [Url]
        public string SharedUrl { get; set; } = string.Empty;

        public IEnumerable<Guid> TagIds { get; set; } = new List<Guid>();
    }
}