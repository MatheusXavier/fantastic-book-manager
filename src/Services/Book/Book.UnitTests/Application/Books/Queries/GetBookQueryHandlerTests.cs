using Book.Application.Books.Models;
using Book.Application.Books.Queries;
using Book.UnitTests.Application.Mocks;

namespace Book.UnitTests.Application.Books.Queries;

public class GetBookQueryHandlerTests
{
    private readonly MockBookRepository _bookRepository;
    private readonly MockLoggedUser _loggedUser;
    private readonly GetBookQueryHandler _handler;

    public GetBookQueryHandlerTests()
    {
        _bookRepository = new();
        _loggedUser = new();
        _handler = new(_bookRepository.Object, _loggedUser.Object);
    }

    [Fact]
    public async Task Handle_BookNotFound_ReturnNull()
    {
        // Arrange
        var query = new GetBookQuery(Guid.NewGuid());
        var userId = Guid.NewGuid();

        _loggedUser
            .MockGetUserId(userId);

        _bookRepository
            .MockGetBookDetailsAsync(query.BookId, userId, book: null);

        // Act
        BookDto? result = await _handler.Handle(query, new CancellationToken());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_BookFound_ReturnBookDetails()
    {
        // Arrange
        var query = new GetBookQuery(Guid.NewGuid());
        var userId = Guid.NewGuid();

        var book = new BookDto(Guid.NewGuid(), "Title1", "Author1", "Genre1");

        _loggedUser
            .MockGetUserId(userId);

        _bookRepository
            .MockGetBookDetailsAsync(query.BookId, userId, book);

        // Act
        BookDto? result = await _handler.Handle(query, new CancellationToken());

        // Assert
        result.Should().BeEquivalentTo(book);
    }
}
