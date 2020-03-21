using System;
using TgStickers.Domain.Enums;

namespace TgStickers.Domain.Entity
{
    public class User
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Login { get; }
        public string Password { get; }
        public Role Role { get; }

        public User(string name, string login, string password, Role role)
        {
            Id = Guid.NewGuid();
            Name = name;
            Login = login;
            Password = password;
            Role = role;
        }
    }
}