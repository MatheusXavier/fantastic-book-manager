using DbUp;
using DbUp.Engine;

using Microsoft.Extensions.Configuration;

using System.Reflection;

namespace Book.Infrastructure.Database.Migrations;

public static class MigrationsManager
{
    public static void MigrateDb(IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("BookDb");

        EnsureDatabase.For.SqlDatabase(connectionString);

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
}
