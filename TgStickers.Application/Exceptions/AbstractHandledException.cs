using System;

namespace TgStickers.Application.Exceptions
{
    public abstract class AbstractHandledException : Exception
    {
        protected AbstractHandledException(string message): base(message)
        {
        }
    }
}