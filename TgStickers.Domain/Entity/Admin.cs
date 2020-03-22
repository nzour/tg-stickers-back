using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TgStickers.Domain.Entity
{
    public class Admin
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Login { get; }
        public string Password { get; }
        public DateTime CreatedAt { get; }
        public IReadOnlyCollection<StickerPack> StickerPacks => new ReadOnlyCollection<StickerPack>(_stickerPacks);

        private readonly IList<StickerPack> _stickerPacks;

        public Admin(string name, string login, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Login = login;
            Password = password;
            CreatedAt = DateTime.UtcNow;
            _stickerPacks = new List<StickerPack>();
        }

        public StickerPack AddNewStickerPack(string name, string sharedUrl, IEnumerable<Tag> tags)
        {
            var stickerPack = new StickerPack(name, sharedUrl, this, tags);

            _stickerPacks.Add(stickerPack);
            return stickerPack;
        }
    }
}