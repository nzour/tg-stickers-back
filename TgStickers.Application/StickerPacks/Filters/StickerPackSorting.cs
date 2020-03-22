using TgStickers.Application.Common;

namespace TgStickers.Application.StickerPacks.Filters
{
    public class StickerPackSorting
    {
        public StickerPackSortingField SortBy { get; set; } = StickerPackSortingField.CreatedAt;
        public SortType SortType { get; set; } = SortType.Descending;
    }

    public enum StickerPackSortingField
    {
        CreatedAt,
        DonationCount,
        ClapCount
    }
}