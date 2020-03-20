using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
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

            services
                .AddInfrastructure(settings)
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new UnixDateTimeConverter());
                    options.SerializerSettings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });
                });
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder
                .UseRouting()
                .UseAuthorization()
                .UseAuthentication()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
