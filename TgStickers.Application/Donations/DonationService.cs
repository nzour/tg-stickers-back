using System;
using System.Linq;
using System.Threading.Tasks;
using TgStickers.Application.Exceptions;
using TgStickers.Domain;
using TgStickers.Domain.Entity;
using TgStickers.Infrastructure.Paginating;

namespace TgStickers.Application.Donations
{
    public class DonationService
    {
        private readonly IRepository<StickerPack> _stickerPackRepository;
        private readonly IRepository<Donation> _donationRepository;

        public DonationService(IRepository<StickerPack> stickerPackRepository, IRepository<Donation> donationRepository)
        {
            _stickerPackRepository = stickerPackRepository;
            _donationRepository = donationRepository;
        }

        public async Task<PaginatedData<DonationOutput>> GetStickerPackDonationsAsync(Guid stickerPackId, Pagination pagination)
        {
            return await _donationRepository.FindAll()
                .Where(donation => stickerPackId == donation.StickerPack.Id)
                .Select(donation => new DonationOutput(donation))
                .PaginateAsync(pagination);
        }

        public async Task<DonationOutput> CreateDonationAsync(DonationInput input)
        {
            var stickerPack = await _stickerPackRepository.FindByIdAsync(input.StickerPackId);

            if (null == stickerPack)
            {
                throw NotFoundException<StickerPack>.WithId(input.StickerPackId);
            }

            var donation = stickerPack.AddDonation(input.SponsorName, input.SponsorEmail, input.Message, input.Money, input.Currency);

            return new DonationOutput(donation);
        }
    }
}
