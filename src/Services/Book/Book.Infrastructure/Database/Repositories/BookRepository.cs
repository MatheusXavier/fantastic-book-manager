using Book.Application.Interfaces;
using Book.Infrastructure.Database.Factories;

using Dapper;

using System.Data;

namespace Book.Infrastructure.Database.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public BookRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task AddBookAsync(Domain.Entities.Book book)
    {
        const string query = @"
            INSERT INTO [dbo].[Books] ([Id],[UserId],[Title],[Author],[Genre])
            VALUES (@Id, @UserId, @Title, @Author, @Genre)";

        using IDbConnection connection = _dbConnectionFactory.GetConnection();

        await connection.ExecuteAsync(query, book);
    }

    public Task<bool> BookExistsAsync(Guid bookId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBookAsync(Guid bookId)
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetBooksCountByTitleAsync(string title, Guid userId)
    {
        const string query = @"
            SELECT COUNT(*)
            FROM [dbo].[Books]
            WHERE [Title] = @Title
              AND [UserId] = @UserId";

        using IDbConnection connection = _dbConnectionFactory.GetConnection();

        return await connection.QueryFirstOrDefaultAsync<int>(query, new
        {
            title,
            userId,
        });
    }
}
