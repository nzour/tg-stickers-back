using Microsoft.Extensions.DependencyInjection;
using TgStickers.Application.Authorization;
using TgStickers.Application.Donations;
using TgStickers.Application.StickerPacks;
using TgStickers.Application.Tags;

namespace TgStickers.Application
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddTransient<AuthorizationService>()
                .AddTransient<StickerPackService>()
                .AddTransient<DonationService>()
                .AddTransient<TagService>();
        }
    }
}