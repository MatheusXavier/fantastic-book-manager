using Book.Application.Interfaces;
using Book.Infrastructure.Database.Repositories;

using Dapper;

using System.Data;

namespace Book.UnitTests.Infrastructure.Database.Repositories;

public class BookRepositoryTests : BaseRepositoryTests
{
    private readonly IBookRepository _bookRepository;

    public BookRepositoryTests()
    {
        _bookRepository = new BookRepository(DbConnectionFactory);
    }

    [Fact]
    public async Task AddBookingAsync_ValidBook_ShouldCreateBookCorrectly()
    {
        // Arrange
        var book = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid(),
            title: "Book title",
            author: "Book author",
            genre: "Book genre");

        // Act
        await _bookRepository.AddBookAsync(book);

        // Assert
        using IDbConnection connection = DbConnectionFactory.GetConnection();

        const string query = "SELECT Id, UserId, Title, Author, Genre FROM Books WHERE Id = @id";

        Book.Domain.Entities.Book foundedBook = await connection
            .QueryFirstOrDefaultAsync<Book.Domain.Entities.Book>(query, new { book.Id });

        foundedBook.Should().NotBeNull();
        foundedBook.UserId.Should().Be(book.UserId);
        foundedBook.Title.Should().Be(book.Title);
        foundedBook.Author.Should().Be(book.Author);
        foundedBook.Genre.Should().Be(book.Genre);
    }

    [Fact]
    public async Task GetBooksCountByTitleAsync_ThereIsNoBookWithTitle_ReturnCountZero()
    {
        // Arrange
        var userId = Guid.NewGuid();
        string title = "Book title";

        // Act
        int result = await _bookRepository.GetBooksCountByTitleAsync(title, userId);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task GetBooksCountByTitleAsync_BookTitleAlreadyRegisteredButForAnotherUser_ReturnCountZero()
    {
        // Arrange
        var book = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid(),
            title: "Book title",
            author: "Book author",
            genre: "Book genre");

        await _bookRepository.AddBookAsync(book);

        // Act
        int result = await _bookRepository.GetBooksCountByTitleAsync(book.Title, userId: Guid.NewGuid());

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task GetBooksCountByTitleAsync_BookTitleAlreadyRegistered_ReturnCountOne()
    {
        // Arrange
        var book = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid(),
            title: "Book title",
            author: "Book author",
            genre: "Book genre");

        await _bookRepository.AddBookAsync(book);

        // Act
        int result = await _bookRepository.GetBooksCountByTitleAsync(book.Title, book.UserId);

        // Assert
        result.Should().Be(1);
    }
}