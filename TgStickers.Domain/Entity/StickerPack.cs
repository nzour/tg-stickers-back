using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TgStickers.Domain.Entity
{
    public class StickerPack
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string SharedUrl { get; set; }
        public uint Claps { get; protected set; }
        public DateTime CreatedAt { get; }
        public Admin CreatedBy { get; }
        public IReadOnlyCollection<Donation> Donations => new ReadOnlyCollection<Donation>(_donations);
        public IReadOnlyCollection<Tag> Tags => new ReadOnlyCollection<Tag>(_tags);

        private readonly IList<Donation> _donations;
        private readonly List<Tag> _tags;

        internal StickerPack(string name, string sharedUrl, Admin createdBy, IEnumerable<Tag> tags)
        {
            Id = Guid.NewGuid();
            Name = name;
            SharedUrl = sharedUrl;
            Claps = 0;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createdBy;
            _donations = new List<Donation>();
            _tags = tags.ToList();
        }

        public void IncreaseClaps(uint clapsCount = 1)
        {
            Claps += clapsCount;
        }

        public Donation AddDonation(string? sponsorName, string? sponsorEmail, string? message, uint money, Currency currency)
        {
            var donation = new Donation(sponsorName, sponsorEmail, message, money, currency, this);

            _donations.Add(donation);

            return donation;
        }

        public void ReplaceTags(IEnumerable<Tag> tags)
        {
            foreach (var tag in _tags)
            {
                tag.RemoveStickerPack(this);
            }

            _tags.Clear();
            _tags.AddRange(tags);
        }
    }
}