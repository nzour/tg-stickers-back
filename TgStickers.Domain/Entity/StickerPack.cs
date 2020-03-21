using System;

namespace TgStickers.Domain.Entity
{
    public class StickerPack
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string SharedUrl { get; set; }
        public uint Claps { get; private set; }
        public DateTime CreatedAt { get; }
        public Admin CreatedBy { get; }

        internal StickerPack(string name, string sharedUrl, Admin createdBy)
        {
            Id = Guid.NewGuid();
            Name = name;
            SharedUrl = sharedUrl;
            Claps = 0;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createdBy;
        }

        public void IncreaseClaps(uint clapsCount = 1)
        {
            Claps += clapsCount;
        }
    }
}