using Book.Application.Books.Models;
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

    public async Task<bool> BookExistsAsync(Guid bookId, Guid userId)
    {
        const string query = @"
            SELECT COUNT(*)
            FROM [dbo].[Books]
            WHERE [Id] = @bookId
              AND [UserId] = @userId";

        using IDbConnection connection = _dbConnectionFactory.GetConnection();

        return (await connection.QueryFirstOrDefaultAsync<int>(query, new
        {
            bookId,
            userId,
        })) > 0;
    }

    public async Task DeleteBookAsync(Guid bookId)
    {
        const string query = "DELETE FROM [dbo].[Books] WHERE [Id] = @bookId";

        using IDbConnection connection = _dbConnectionFactory.GetConnection();

        await connection.ExecuteAsync(query, new { bookId });
    }

    public async Task<Domain.Entities.Book?> GetBookAsync(Guid bookId)
    {
        const string query = @"SELECT Id, UserId, Title, Author, Genre 
            FROM [dbo].[Books] 
            WHERE [Id] = @bookId";

        using IDbConnection connection = _dbConnectionFactory.GetConnection();

        return await connection
            .QueryFirstOrDefaultAsync<Domain.Entities.Book>(query, new
            {
                bookId
            });
    }

    public async Task UpdateBookAsync(Domain.Entities.Book book)
    {
        const string query = @"UPDATE [dbo].[Books] 
            SET [Title] = @Title, [Author] = @Author, [Genre] = @Genre 
            WHERE Id = @Id";

        using IDbConnection connection = _dbConnectionFactory.GetConnection();

        await connection.ExecuteAsync(query, book);
    }

    public Task<List<BookDto>> GetBooksByUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}
