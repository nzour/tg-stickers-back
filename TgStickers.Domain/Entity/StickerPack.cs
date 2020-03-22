using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        private readonly IList<Donation> _donations;

        internal StickerPack(string name, string sharedUrl, Admin createdBy)
        {
            Id = Guid.NewGuid();
            Name = name;
            SharedUrl = sharedUrl;
            Claps = 0;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createdBy;
            _donations = new List<Donation>();
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
    }
}