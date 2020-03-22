using TgStickers.Application.Common;

namespace TgStickers.Application.Tags
{
    public class TagNameFilter
    {
        public string Name { get; set; } = string.Empty;
        public SearchType SearchType { get; set; } = SearchType.Equals;
        public bool HasValue => string.Empty != Name.Trim();
    }
}