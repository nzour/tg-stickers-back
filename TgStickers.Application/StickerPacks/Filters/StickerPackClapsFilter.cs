using TgStickers.Application.Common;

namespace TgStickers.Application.StickerPacks.Filters
{
    public class StickerPackClapsFilter
    {
        public uint? ClapsCount { get; set; }
        public SearchType ClapsSearchType { get; set; } = SearchType.GreaterOrEquals;

        public bool HasValue => 0 != ClapsCount;
    }
}