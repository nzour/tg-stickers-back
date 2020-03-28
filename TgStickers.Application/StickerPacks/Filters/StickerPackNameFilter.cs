using TgStickers.Application.Common;

namespace TgStickers.Application.StickerPacks.Filters
{
    public class StickerPackNameFilter
    {
        public string? Name { get; set; }
        public SearchType NameSearchType { get; set; } = SearchType.Equals;
    }
}