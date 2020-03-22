using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TgStickers.Domain.Entity
{
    public class Tag
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public IReadOnlyCollection<StickerPack> StickerPacks => new ReadOnlyCollection<StickerPack>(_stickerPacks);

        private readonly IList<StickerPack> _stickerPacks;

        public Tag(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            _stickerPacks = new List<StickerPack>();
        }

        internal void AddStickerPack(StickerPack stickerPack)
        {
            _stickerPacks.Add(stickerPack);
        }

        internal void RemoveStickerPack(StickerPack stickerPack)
        {
            _stickerPacks.Remove(stickerPack);
        }
    }
}