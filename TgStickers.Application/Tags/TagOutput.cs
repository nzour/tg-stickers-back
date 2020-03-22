using System;
using TgStickers.Domain.Entity;

namespace TgStickers.Application.Tags
{
    public class TagOutput
    {
        public Guid Id { get; }
        public string Name { get; }

        public TagOutput(Tag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
        }
    }
}