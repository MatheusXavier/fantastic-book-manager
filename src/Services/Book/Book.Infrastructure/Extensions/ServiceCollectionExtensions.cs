using Book.Application.Interfaces;
using Book.Infrastructure.Behavior;
using Book.Infrastructure.Database.Factories;
using Book.Infrastructure.Database.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace Book.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        return services
            .AddRepositories()
            .AddBehaviors();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDbConnectionFactory, DbConnectionFactory>()
            .AddScoped<IBookRepository, BookRepository>();
    }

    private static IServiceCollection AddBehaviors(this IServiceCollection services)
    {
        return services
            .AddScoped<IErrorHandler, ErrorHandler>();
    }
}
