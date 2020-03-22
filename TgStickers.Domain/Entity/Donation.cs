using System;

namespace TgStickers.Domain.Entity
{
    public class Donation
    {
        public Guid Id { get; }
        public string? SponsorName { get; }
        public string? SponsorEmail { get; }
        public string? Message { get; }
        public uint Money { get; }
        public Currency Currency { get; }
        public DateTime CreatedAt { get; }
        public StickerPack StickerPack { get; }

        internal Donation(string? sponsorName, string? sponsorEmail, string? message, uint money, Currency currency, StickerPack stickerPack)
        {
            Id = Guid.NewGuid();
            SponsorName = sponsorName;
            SponsorEmail = sponsorEmail;
            Message = message;
            Money = money;
            Currency = currency;
            CreatedAt = DateTime.UtcNow;
            StickerPack = stickerPack;
        }
    }
}