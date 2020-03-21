using Microsoft.AspNetCore.Mvc;

namespace TgStickers.Api.Controllers
{
    [Route("foobar")]
    public class FoobarController : Controller
    {
        [HttpGet("beep")]
        public string Beep()
        {
            return "boop";
        }
    }
}
