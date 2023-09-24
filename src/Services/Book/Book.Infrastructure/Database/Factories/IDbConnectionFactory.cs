using System.Data;

namespace Book.Infrastructure.Database.Factories;

public interface IDbConnectionFactory
{
    IDbConnection GetConnection();
}