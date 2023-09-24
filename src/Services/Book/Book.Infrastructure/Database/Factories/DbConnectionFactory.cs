using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using System.Data;

namespace Book.Infrastructure.Database.Factories;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BookDb")
            ?? throw new InvalidOperationException("Could not found BookDb connection string");
    }

    public IDbConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}