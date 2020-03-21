using BCryptGenerator = BCrypt.Net.BCrypt;

namespace TgStickers.Infrastructure.Security
{
    public class BCryptPasswordEncoder : IPasswordEncoder
    {
        public string Encode(string password)
        {
            return BCryptGenerator.HashPassword(password);
        }

        public bool VerifyPassword(string passwordAsPlainText, string hashOfPassword)
        {
            return BCryptGenerator.Verify(passwordAsPlainText, hashOfPassword);
        }
    }
}