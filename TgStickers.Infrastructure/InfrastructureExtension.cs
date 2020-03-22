using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NHibernate;
using TgStickers.Domain;
using TgStickers.Domain.Entity;
using TgStickers.Infrastructure.Jwt;
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
                .AddBCryptPasswordEncoder()
                .AddDefaultJwtManager(settings.JwtSettings)
                .AddJwtAuthentication(settings.JwtSettings);
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
                .AddTransient<IRepository<StickerPack>, NHibernateRepository<StickerPack>>()
                .AddTransient<IRepository<Donation>, NHibernateRepository<Donation>>()
                .AddTransient<IRepository<Tag>, NHibernateRepository<Tag>>();
        }

        public static IServiceCollection AddBCryptPasswordEncoder(this IServiceCollection services)
        {
            return services.AddTransient<IPasswordEncoder, BCryptPasswordEncoder>();
        }

        public static IServiceCollection AddDefaultJwtManager(this IServiceCollection services, JwtSettings settings)
        {
            return services.AddTransient<IJwtManager>(_ => new DefaultJwtManager(settings.SecretKey, settings.TokenTtl));
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtSettings settings)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var key = Encoding.UTF8.GetBytes(settings.SecretKey);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime =  true,
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience =  false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            return services;
        }
    }
}