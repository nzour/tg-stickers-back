using System;
using System.Linq;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TgStickers.Infrastructure;

namespace TgStickers.Migrations
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Specify action:");
            Console.WriteLine("'list' - list of migraitons");
            Console.WriteLine("'migrate' - migrate up to latest version");
            Console.WriteLine("'downgrade' - downgrade to specified version");

            var action = args.FirstOrDefault() ?? Console.ReadLine();

            Action runAction = action switch
            {
                "list" => () => CreateRunner().ListMigrations(),
                "migrate" => () => CreateRunner().MigrateUp(),
                "downgrade" => () => CreateDownGrader(args),
                _ => () => Console.WriteLine($"Unrecognozed action {action}")
            };

            runAction.Invoke();
        }

        private static IMigrationRunner CreateRunner()
        {
            var settings = new NHibernateSettings();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            configuration.Bind("Postgres", settings);

            var scope = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(runner =>
                    runner
                        .AddPostgres()
                        .WithGlobalConnectionString(settings.ConnectionString)
                        .ScanIn(typeof(Program).Assembly).For.Migrations())
                .AddLogging(logging =>
                    logging.Services
                        .AddSingleton<ILoggerProvider, FluentMigratorConsoleLoggerProvider>()
                        .Configure<FluentMigratorLoggerOptions>(options =>
                        {
                            options.ShowSql = true;
                            options.ShowElapsedTime = true;
                        }))
                .BuildServiceProvider()
                .CreateScope();

            return scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        }

        private static Action CreateDownGrader(string[] args)
        {
            string? version;

            if (args.Length > 1)
            {
                version = args[1];
            }
            else
            {
                Console.Write("Spicify version to downgrade: ");
                version = Console.ReadLine();
            }

            return () => CreateRunner().MigrateDown(long.Parse(version));
        }
    }
}