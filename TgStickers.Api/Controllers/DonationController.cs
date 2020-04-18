using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TgStickers.Application.Donations;
using TgStickers.Infrastructure.Paginating;
using TgStickers.Infrastructure.Transaction;

namespace TgStickers.Api.Controllers
{
    [Route("donations"), AllowAnonymous]
    public class DonationController : Controller
    {
        private readonly DonationService _donationService;
        private readonly ITransactional _transactional;

        public DonationController(DonationService donationService, ITransactional transactional)
        {
            _donationService = donationService;
            _transactional = transactional;
        }

        [HttpGet("for-stickers/{stickerPackId:guid}")]
        public async Task<PaginatedData<DonationOutput>> GetStickerPackDonationsAsync([FromRoute] Guid stickerPackId, [FromQuery] Pagination pagination)
        {
            return await _donationService.GetStickerPackDonationsAsync(stickerPackId, pagination);
        }

        [HttpPost]
        public async Task<DonationOutput> CreateDonationAsync([FromBody] DonationInput input)
        {
            return await _transactional.ExecuteAsync(async () => await _donationService.CreateDonationAsync(input));
        }
    }
}
