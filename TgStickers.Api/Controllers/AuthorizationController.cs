using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TgStickers.Application.Authorization;
using TgStickers.Infrastructure.Transaction;

namespace TgStickers.Api.Controllers
{
    [Route("auth"), AllowAnonymous]
    public class AuthorizationController : Controller
    {
        private readonly ITransactional _transactional;
        private readonly AuthorizationService _authorizationService;

        public AuthorizationController(ITransactional transactional, AuthorizationService authorizationService)
        {
            _transactional = transactional;
            _authorizationService = authorizationService;
        }

        [HttpPost("register")]
        public async Task<AdminTokenOutput> RegisterAsync([FromBody] RegisterInput input)
        {
            return await _transactional.DoAsync(async () => await _authorizationService.RegisterAsync(input));
        }

        [HttpPost("login")]
        public async Task<AdminTokenOutput> LogInAsync([FromBody] LogInInput input)
        {
            // Если решусь добавить refresh-токен, то надо не забыть обернуть в транзакцию (т.к. refresh-токен буду хранить в базе)
            return await _authorizationService.LogInAsync(input);
        }
    }
}