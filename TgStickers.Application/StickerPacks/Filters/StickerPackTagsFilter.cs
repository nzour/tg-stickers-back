using System;
using System.Collections.Generic;
using FluentNHibernate.Conventions;

namespace TgStickers.Application.StickerPacks.Filters
{
    public class StickerPackTagsFilter
    {
        public IEnumerable<Guid> TagIds { get; set; } = new List<Guid>();

        public bool HasValue => TagIds.IsNotEmpty();
    }
}