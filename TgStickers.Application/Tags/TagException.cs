using TgStickers.Application.Exceptions;

namespace TgStickers.Application.Tags
{
    public class TagException : AbstractHandledException
    {
        public TagException(string message) : base(message)
        {
        }

        public static TagException NameIsBusy(string name)
        {
            return new TagException($"Tag name '{name}' is busy!");
        }
    }
}
