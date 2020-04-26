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
        public string? Alias { get; set; }
        public int Claps { get; set; }
        public DateTime CreatedAt { get; set; }
        public AdminOutput CreatedBy { get; set; }
        public IEnumerable<TagOutput> Tags { get; set; }
        public string FirstStickerPath { get; set; }
        public int StickersCount { get; set; }

        public StickerPackOutput(StickerPack stickerPack, string firstStickerPath, int stickersCount)
        {
            Id = stickerPack.Id;
            Name = stickerPack.Name;
            Alias = stickerPack.Alias;
            Claps = stickerPack.Claps;
            CreatedAt = stickerPack.CreatedAt;
            CreatedBy = new AdminOutput(stickerPack.CreatedBy);
            Tags = stickerPack.Tags.Select(t => new TagOutput(t)).ToList();
            FirstStickerPath = firstStickerPath;
            StickersCount = stickersCount;
        }
    }
}
