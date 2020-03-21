using System;
using TgStickers.Domain.Entity;

namespace TgStickers.Application.Authorization
{
    public class AdminTokenOutput
    {
        public Guid AdminId { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string AccessToken { get; set; }

        public AdminTokenOutput(Admin admin, string accessToken)
        {
            AdminId = admin.Id;
            Name = admin.Name;
            Login = admin.Login;
            AccessToken = accessToken;
        }
    }
}