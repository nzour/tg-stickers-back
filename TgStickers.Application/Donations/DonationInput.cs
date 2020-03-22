using System;
using TgStickers.Domain;

namespace TgStickers.Application.Donations
{
    public class DonationInput
    {
        public string? SponsorName { get; set; }
        public string? SponsorEmail { get; set; }
        public string? Message { get; set; }
        public uint Money { get; set; }
        public Currency Currency { get; set; }
        public Guid StickerPackId { get; set; } = Guid.Empty;
    }
}
