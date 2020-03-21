using TgStickers.Domain.Entity;

namespace TgStickers.Infrastructure.Jwt
{
    public interface IJwtManager
    {
        string CreateToken(Admin admin);
    }
}