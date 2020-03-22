using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TgStickers.Api.Controllers
{
    [Route("foobar"), Authorize]
    public class FoobarController : Controller
    {
        [HttpGet("beep")]
        public string Beep()
        {
            return "boop";
        }
    }
}
