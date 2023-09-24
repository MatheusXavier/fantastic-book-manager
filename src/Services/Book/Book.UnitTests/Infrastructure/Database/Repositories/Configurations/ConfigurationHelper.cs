using Microsoft.Extensions.Configuration;

namespace Book.UnitTests.Infrastructure.Database.Repositories.Configurations;

public static class ConfigurationHelper
{
    public static string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

    static ConfigurationHelper()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", false)
                .AddEnvironmentVariables();

        Configuration = builder.Build();
    }

    public static IConfiguration Configuration { get; }
}
