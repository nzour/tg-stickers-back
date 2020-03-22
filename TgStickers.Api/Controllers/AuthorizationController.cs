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
            return await _transactional.ExecuteAsync(async () => await _authorizationService.RegisterAsync(input));
        }

        [HttpPost("login")]
        public async Task<AdminTokenOutput> LogInAsync([FromBody] LogInInput input)
        {
            // Если решусь добавить refresh-токен, то надо не забыть обернуть в транзакцию (т.к. refresh-токен буду хранить в базе)
            return await _authorizationService.LogInAsync(input);
        }

        [HttpHead("login-busy")]
        public async Task<StatusCodeResult> IsLoginBusyAsync([FromQuery] string login)
        {
            return await _authorizationService.IsLoginBusyAsync(login)
                ? new StatusCodeResult(200)
                : new StatusCodeResult(404);
        }
    }
}