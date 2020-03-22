using System.ComponentModel.DataAnnotations;

namespace TgStickers.Application.Authorization
{
    public class RegisterInput
    {
        [MinLength(1, ErrorMessage = "Property 'name' must be not blank")]
        public string Name { get; set; } = string.Empty;

        [MinLength(7, ErrorMessage = "Property 'login' must contains more than 6 characters")]
        public string Login { get; set; } = string.Empty;

        [MinLength(7, ErrorMessage = "Property 'password' must contain more than 6 characters")]
        public string Password { get; set; } = string.Empty;
    }
}