using System;

namespace TgStickers.Domain.Entity
{
    public class Tag
    {
        public Guid Id { get; }
        public string Name { get; set; }

        public Tag(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}