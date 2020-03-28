using TgStickers.Application.Common;

namespace TgStickers.Application.Tags
{
    public class TagNameFilter
    {
        public string? Name { get; set; }
        public SearchType SearchType { get; set; } = SearchType.Equals;
    }
}