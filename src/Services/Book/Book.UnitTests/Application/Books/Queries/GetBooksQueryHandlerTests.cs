using Book.Application.Books.Models;
using Book.UnitTests.Application.Mocks;

namespace Book.UnitTests.Application.Books.Queries;

public class GetBooksQueryHandlerTests
{
    private readonly MockBookRepository _bookRepository;
    private readonly MockLoggedUser _loggedUser;
    private readonly GetBooksQueryHandler _handler;

    public GetBooksQueryHandlerTests()
    {
        _bookRepository = new();
        _loggedUser = new();
        _handler = new(_bookRepository.Object, _loggedUser.Object);
    }

    [Fact]
    public async Task Handle_GettingUserId_ReturnBooks()
    {
        // Arrange
        var query = new GetBooksQuery();
        var userId = Guid.NewGuid();
        var books = new List<BookDto>()
        {
            new BookDto(Guid.NewGuid(), "Title1", "Author1", "Genre1"),
            new BookDto(Guid.NewGuid(), "Title2", "Author2", "Genre2"),
        };

        _loggedUser
            .MockGetUserId(userId);

        _bookRepository
            .MockGetBooksByUserAsync(userId, books);

        // Act
        List<BookDto> result = await _handler.Handle(query, new CancellationToken());

        // Assert
        result.Should().BeEquivalentTo(books);
    }
}
