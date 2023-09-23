using Microsoft.EntityFrameworkCore;

using NetDevPack.Identity.Jwt;

namespace Identity.API.Infrastructure.Extensions;

public static class InfraServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add default EF Context for Identity from 'NetDevPack.Identity'
        services
            .AddIdentityEntityFrameworkContextConfiguration(o =>
            {
                o.UseSqlServer(
                    configuration.GetConnectionString("IdentityDb"),
                    b => b.MigrationsAssembly("Identity.API"));
            });

        // Add default identity configuration from 'NetDevPack.Identity'
        services.AddIdentityConfiguration();

        // Add default JWT configuration from 'NetDevPack.Identity'
        services.AddJwtConfiguration(configuration, "IdentitySettings");

        return services;
    }
}