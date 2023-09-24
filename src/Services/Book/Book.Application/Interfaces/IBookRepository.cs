namespace Book.Application.Interfaces;

public interface IBookRepository
{
    Task<int> GetBooksCountByTitleAsync(string title, Guid userId);

    Task AddBookAsync(Domain.Entities.Book book);

    Task<bool> BookExistsAsync(Guid bookId, Guid userId);

    Task DeleteBookAsync(Guid bookId);
}
