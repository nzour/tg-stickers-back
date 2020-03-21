using TgStickers.Application.Exceptions;

namespace TgStickers.Application.Authorization
{
    public class AuthorizationException : AbstractHandledException
    {
        public AuthorizationException(string message) : base(message)
        {
        }

        public static AuthorizationException LoginIsBusy(string login)
        {
            return new AuthorizationException($"Login '{login}' is busy!");
        }

        public static AuthorizationException BadCredentials()
        {
            return new AuthorizationException("No such admin with speicified login and password was found!");
        }
    }
}