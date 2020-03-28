using System;
using System.Collections.Generic;
using System.Linq;
using TgStickers.Application.Common;
using TgStickers.Application.Tags;
using TgStickers.Domain.Entity;

namespace TgStickers.Application.StickerPacks
{
    public class StickerPackOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SharedUrl { get; set; }
        public uint Claps { get; set; }
        public DateTime CreatedAt { get; set; }
        public AdminOutput CreatedBy { get; set; }
        public IEnumerable<TagOutput> Tags { get; set; }

        public StickerPackOutput(StickerPack stickerPack)
        {
            Id = stickerPack.Id;
            Name = stickerPack.Name;
            SharedUrl = stickerPack.SharedUrl;
            Claps = stickerPack.Claps;
            CreatedAt = stickerPack.CreatedAt;
            CreatedBy = new AdminOutput(stickerPack.CreatedBy);
            Tags = stickerPack.Tags.Select(t => new TagOutput(t)).ToList();
        }
    }
}