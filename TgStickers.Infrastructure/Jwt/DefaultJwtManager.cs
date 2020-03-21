using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TgStickers.Domain.Entity;
using Microsoft.IdentityModel.Tokens;

namespace TgStickers.Infrastructure.Jwt
{
    public class DefaultJwtManager : IJwtManager
    {
        private readonly string _secretKey;
        private readonly uint _tokenTtl;

        public DefaultJwtManager(string secretKey, uint tokenTtl)
        {
            _secretKey = secretKey;
            _tokenTtl = tokenTtl;
        }

        public string CreateToken(Admin admin)
        {
            var secretKet = Encoding.UTF8.GetBytes(_secretKey);

            var claims = new List<Claim>
            {
                new Claim("AdminId", admin.Id.ToString()),
                new Claim("Login", admin.Login)
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(_tokenTtl),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKet), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(tokenHandler.CreateToken(descriptor));
        }
    }
}