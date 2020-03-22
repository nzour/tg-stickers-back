using System;
using TgStickers.Domain;
using TgStickers.Domain.Entity;

namespace TgStickers.Application.Donations
{
    public class DonationOutput
    {
        public Guid Id { get; set; }
        public string? SponsorName { get; set; }
        public string? SponsorEmail { get; set; }
        public string? Message { get; set; }
        public uint Money { get; set; }
        public Currency Currency { get; set; }
        public DateTime CreatedAt { get; set; }

        public DonationOutput(Donation donation)
        {
            Id = donation.Id;
            SponsorName = donation.SponsorName;
            SponsorEmail = donation.SponsorEmail;
            Message = donation.Message;
            Money = donation.Money;
            Currency = donation.Currency;
            CreatedAt = donation.CreatedAt;
        }
    }
}
