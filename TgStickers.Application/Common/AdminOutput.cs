using System;
using TgStickers.Domain.Entity;

namespace TgStickers.Application.Common
{
    public class AdminOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public DateTime CreatedAt { get; set; }

        public AdminOutput(Admin admin)
        {
            Id = admin.Id;
            Name = admin.Name;
            Login = admin.Login;
            CreatedAt = admin.CreatedAt;
        }
    }
}