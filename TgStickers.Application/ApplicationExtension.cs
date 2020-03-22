using Microsoft.Extensions.DependencyInjection;
using TgStickers.Application.Authorization;
using TgStickers.Application.Tags;

namespace TgStickers.Application
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddTransient<AuthorizationService>()
                .AddTransient<TagService>();
        }
    }
}