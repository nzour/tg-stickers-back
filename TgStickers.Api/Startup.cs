using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Converters;
using TgStickers.Api.Configuration;
using TgStickers.Api.Services;
using TgStickers.Application;
using TgStickers.Infrastructure;

namespace TgStickers.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var settings = new InfrastructureSettings();

            _configuration.Bind("Postgres", settings.NHibernateSettings);
            _configuration.Bind("Jwt", settings.JwtSettings);
            _configuration.Bind("Telegram", settings.TelegramSettings);

            services
                .AddInfrastructure(settings)
                .AddApplicationServices()
                .AddHttpContextAccessor()
                .AddTransient<CurrentAdminProvider>()
                .AddTransient<ExceptionHandlingMiddleware>()
                .AddControllers()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<ModelValidationFilter>();
                    options.Filters.Add(new AuthorizeFilter());
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new UnixDateTimeConverter());
                    options.SerializerSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });
                });
        }

        public void Configure(IApplicationBuilder applicationBuilder) =>
            applicationBuilder
                .UseCors(config =>
                    config
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin())
                .UseMiddleware<ExceptionHandlingMiddleware>()
                .UseAuthentication()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers())
                .UseStaticFiles(new StaticFileOptions
                {
                    RequestPath = "/files",
                    FileProvider = new PhysicalFileProvider(root: _configuration["Telegram:DirectoryToSaveImages"])
                });
    }
}
