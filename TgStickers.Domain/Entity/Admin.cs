using System;

namespace TgStickers.Domain.Entity
{
    public class Admin
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Login { get; }
        public string Password { get; }
        public DateTime CreatedAt { get; }

        public Admin(string name, string login, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Login = login;
            Password = password;
            CreatedAt = DateTime.UtcNow;
        }
    }
}