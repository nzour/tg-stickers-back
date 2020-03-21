using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TgStickers.Application.Exceptions;
using TgStickers.Domain;
using TgStickers.Domain.Entity;

namespace TgStickers.Api.Services
{
    public class CurrentAdminProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRepository<Admin> _adminRepository;

        public CurrentAdminProvider(IHttpContextAccessor contextAccessor, IRepository<Admin> adminRepository)
        {
            _contextAccessor = contextAccessor;
            _adminRepository = adminRepository;
        }

        public async Task<Admin> ProviderCurrentAdminAsync()
        {
            var adminId = ExtractCurrentAdminId();

            return await _adminRepository.FindByIdAsync(adminId) ?? throw NotFoundException<Admin>.WithId(adminId);
        }

        private Guid ExtractCurrentAdminId()
        {
            var adminId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "AdminId")?.Value
                          ?? throw new AuthenticationException("Action requires authenticated admin, but there is not such admin!");

            return Guid.Parse(adminId);
        }
    }
}