using Identity.API.Infrastructure.Logging;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Polly;
using Polly.Retry;

namespace Identity.API.Infrastructure.Extensions;

public static class InfraWebApplicationExtensions
{
    public static WebApplication MigrateDbContext<TContext>(
        this WebApplication app,
        Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
        using IServiceScope scope = app.Services.CreateScope();

        IServiceProvider services = scope.ServiceProvider;
        ILogger<TContext> logger = services.GetRequiredService<ILogger<TContext>>();
        TContext context = services.GetRequiredService<TContext>();

        try
        {
            logger.LogInformationMessage<TContext>($"Migrating database...");

            RetryPolicy retry = Policy.Handle<SqlException>()
                 .WaitAndRetry(new TimeSpan[]
                 {
                     TimeSpan.FromSeconds(3),
                     TimeSpan.FromSeconds(5),
                     TimeSpan.FromSeconds(8),
                 });

            //if the sql server container is not created on run docker compose this
            //migration can't fail for network related exception. The retry options for DbContext only
            //apply to transient exceptions
            retry.Execute(() => InvokeSeeder(seeder, context, services));
        }
        catch (SqlException ex)
        {
            logger.LogException<TContext>(ex);
        }

        return app;
    }

    private static void InvokeSeeder<TContext>(
        Action<TContext, IServiceProvider> seeder,
        TContext context,
        IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}