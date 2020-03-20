using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace TgStickers.Infrastructure.NHibernate
{
    public static class NHibernateFactory
    {
        public static ISessionFactory CreateSessionFactory(NHibernateSettings settings)
        {
            var configuration = new Configuration().SetNamingStrategy(new PostgresNamingStrategy());

            return Fluently.Configure(configuration)
                .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(settings.ConnectionString))
                .Mappings(ConfigureMappings)
                .BuildSessionFactory();
        }

        private static void ConfigureMappings(MappingConfiguration mappings)
        {
            mappings.FluentMappings.AddFromAssembly(typeof(NHibernateFactory).Assembly);
        }
    }
}
