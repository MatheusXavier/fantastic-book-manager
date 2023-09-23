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
}