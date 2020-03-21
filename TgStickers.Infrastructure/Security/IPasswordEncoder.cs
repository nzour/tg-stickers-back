namespace TgStickers.Infrastructure.Security
{
    public interface IPasswordEncoder
    {
        string Encode(string password);
        bool VerifyPassword(string passwordAsPlainText, string hashOfPassword);
    }
}