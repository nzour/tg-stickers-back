using TgStickers.Application.Common;

namespace TgStickers.Application.StickerPacks.Filters
{
    public class StickerPackClapsFilter
    {
        public uint? ClapsCount { get; set; }
        public SearchType SearchType { get; set; } = SearchType.GreaterOrEquals;

        public bool HasValue => 0 != ClapsCount;
    }
}