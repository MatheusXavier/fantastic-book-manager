using DbUp;
using DbUp.Engine;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Polly;
using Polly.Retry;

using System.Reflection;

namespace Book.Infrastructure.Database.Migrations;

public static class MigrationsManager
{
    public static void MigrateDb(IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("BookDb") ??
            throw new InvalidOperationException("Could not found book db connection string");

        EnsureDatabaseExists(connectionString);

        UpgradeEngine upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToAutodetectedLog()
            .Build();

        DatabaseUpgradeResult result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            throw new InvalidOperationException("Could not apply migrations to database");
        }
    }

    private static void EnsureDatabaseExists(string connectionString)
    {
        RetryPolicy retry = Policy.Handle<SqlException>()
            .WaitAndRetry(new TimeSpan[]
            {
                TimeSpan.FromSeconds(3),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(8),
            });

        //if the sql server container is not created on run docker compose this
        //migration can't fail for network related exception. 
        retry.Execute(() => EnsureDatabase.For.SqlDatabase(connectionString));
    }
}
