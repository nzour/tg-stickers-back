namespace TgStickers.Application.Exceptions
{
    public class NotFoundException<T> : AbstractHandledException where T : class
    {
        public NotFoundException(string message): base(message)
        {
        }

        public static NotFoundException<T> WithId(object id)
        {
            return new NotFoundException<T>($"Entity '{typeof(T).Name}' with id '{id}' was not found.");
        }
    }
}