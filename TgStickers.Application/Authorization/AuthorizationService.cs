using System.Linq;
using System.Threading.Tasks;
using NHibernate.Linq;
using TgStickers.Domain;
using TgStickers.Domain.Entity;
using TgStickers.Infrastructure.Jwt;
using TgStickers.Infrastructure.Security;

namespace TgStickers.Application.Authorization
{
    public class AuthorizationService
    {
        private readonly IRepository<Admin> _adminRepository;
        private readonly IPasswordEncoder _passwordEncoder;
        private readonly IJwtManager _jwtManager;

        public AuthorizationService(IRepository<Admin> adminRepository, IPasswordEncoder passwordEncoder, IJwtManager jwtManager)
        {
            _adminRepository = adminRepository;
            _passwordEncoder = passwordEncoder;
            _jwtManager = jwtManager;
        }

        public async Task<AdminTokenOutput> RegisterAsync(RegisterInput input)
        {
            if (await IsLoginBusyAsync(input.Login))
            {
                throw AuthorizationException.LoginIsBusy(input.Login);
            }

            var password = _passwordEncoder.Encode(input.Password);
            var admin = new Admin(input.Name, input.Login, password);

            await _adminRepository.SaveAsync(admin);

            return new AdminTokenOutput(admin, _jwtManager.CreateToken(admin));
        }

        public async Task<AdminTokenOutput> LogInAsync(LogInInput input)
        {
            var admin = await _adminRepository.FindAll()
                .Where(x => x.Login == input.Login)
                .FirstOrDefaultAsync();

            bool DoesNotMatchPasswords() => false == _passwordEncoder.VerifyPassword(input.Password, admin!.Password);

            if (null == admin || DoesNotMatchPasswords())
            {
                throw AuthorizationException.BadCredentials();
            }

            return new AdminTokenOutput(admin, _jwtManager.CreateToken(admin));
        }

        public async Task<bool> IsLoginBusyAsync(string login)
        {
            return 0 != await _adminRepository.FindAll()
                .Where(admin => login == admin.Login)
                .CountAsync();
        }
    }
}