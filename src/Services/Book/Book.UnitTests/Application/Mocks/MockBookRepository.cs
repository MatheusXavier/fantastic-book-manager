using Book.Application.Books.Models;
using Book.Application.Interfaces;

using Moq;

namespace Book.UnitTests.Application.Mocks;

public class MockBookRepository : Mock<IBookRepository>
{
    public MockBookRepository() : base(MockBehavior.Strict) { }

    public MockBookRepository MockGetBooksCountByTitleAsync(string title, Guid userId, int count)
    {
        Setup(s => s.GetBooksCountByTitleAsync(title, userId))
            .ReturnsAsync(count);

        return this;
    }

    public MockBookRepository MockAddBookAsync(Book.Domain.Entities.Book book)
    {
        Setup(s => s.AddBookAsync(book))
            .Returns(Task.CompletedTask);

        return this;
    }

    public MockBookRepository MockBookExistsAsync(Guid bookId, Guid userId, bool result)
    {
        Setup(s => s.BookExistsAsync(bookId, userId))
            .ReturnsAsync(result);

        return this;
    }

    public MockBookRepository MockDeleteBookAsync(Guid bookId)
    {
        Setup(s => s.DeleteBookAsync(bookId))
            .Returns(Task.CompletedTask);

        return this;
    }

    public MockBookRepository MockGetBookAsync(Guid bookId, Book.Domain.Entities.Book? book)
    {
        Setup(s => s.GetBookAsync(bookId))
            .ReturnsAsync(book);

        return this;
    }

    public MockBookRepository MockUpdateBookAsync(Book.Domain.Entities.Book book)
    {
        Setup(s => s.UpdateBookAsync(book))
            .Returns(Task.CompletedTask);

        return this;
    }

    public MockBookRepository MockGetBooksByUserAsync(Guid userId, List<BookDto> books)
    {
        Setup(s => s.GetBooksByUserAsync(userId))
            .ReturnsAsync(books);

        return this;
    }

    public MockBookRepository MockGetBookDetailsAsync(Guid bookId, Guid userId, BookDto? book)
    {
        Setup(s => s.GetBookDetailsAsync(bookId, userId))
            .ReturnsAsync(book);

        return this;
    }
}