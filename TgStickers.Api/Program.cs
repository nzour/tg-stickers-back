using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace TgStickers.Api
{
    internal class Program
    {
        public static void Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(ConfigureWebHost)
                .Build()
                .Run();

        public static void ConfigureWebHost(IWebHostBuilder webHostBuilder) =>
            webHostBuilder.UseStartup<Startup>().UseConfiguration(CreateConfiguration());

        public static IConfiguration CreateConfiguration() =>
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
    }
}
