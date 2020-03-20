using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using TgStickers.Infrastructure.NHibernate;
using TgStickers.Infrastructure.Transaction;

namespace TgStickers.Infrastructure
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, InfrastructureSettings settings)
        {
            return services.AddNHibernate(settings.NHibernateSettings);
        }

        public static IServiceCollection AddNHibernate(this IServiceCollection services, NHibernateSettings settings)
        {
            ISessionFactory sessionFactory = NHibernateFactory.CreateSessionFactory(settings);

            return services
                .AddSingleton(sessionFactory)
                .AddScoped(_ => sessionFactory.OpenSession())
                .AddScoped<ITransactional, NHibernateTransactional>();
        }
    }
}