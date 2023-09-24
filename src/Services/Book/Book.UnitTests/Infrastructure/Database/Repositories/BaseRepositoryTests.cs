using Book.Infrastructure.Database.Factories;
using Book.UnitTests.Infrastructure.Database.Repositories.Configurations;
using System.Transactions;

namespace Book.UnitTests.Infrastructure.Database.Repositories;

[AutoRollbackTransaction(IsolationLevel = IsolationLevel.ReadUncommitted)]
public class BaseRepositoryTests
{
    public IDbConnectionFactory DbConnectionFactory { get; private set; }

    public BaseRepositoryTests()
    {
        DbConnectionFactory = new DbConnectionFactory(ConfigurationHelper.Configuration);
    }
}
