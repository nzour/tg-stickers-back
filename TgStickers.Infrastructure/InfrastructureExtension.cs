using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using TgStickers.Domain;
using TgStickers.Domain.Entity;
using TgStickers.Infrastructure.NHibernate;
using TgStickers.Infrastructure.Security;
using TgStickers.Infrastructure.Transaction;

namespace TgStickers.Infrastructure
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, InfrastructureSettings settings)
        {
            return services
                .AddNHibernate(settings.NHibernateSettings)
                .AddRepositories()
                .AddSecurity();
        }

        public static IServiceCollection AddNHibernate(this IServiceCollection services, NHibernateSettings settings)
        {
            ISessionFactory sessionFactory = NHibernateFactory.CreateSessionFactory(settings);

            return services
                .AddSingleton(sessionFactory)
                .AddScoped(_ => sessionFactory.OpenSession())
                .AddScoped<ITransactional, NHibernateTransactional>();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient<IRepository<Admin>, NHibernateRepository<Admin>>()
                .AddTransient<IRepository<StickerPack>, NHibernateRepository<StickerPack>>();
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            return services.AddTransient<IPasswordEncoder, BCryptPasswordEncoder>();
        }
    }
}