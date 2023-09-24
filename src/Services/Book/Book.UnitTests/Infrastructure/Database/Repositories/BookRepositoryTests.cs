using Book.Application.Books.Models;
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

    [Fact]
    public async Task BookExistsAsync_WithInexistentBook_ReturnFalse()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        bool result = await _bookRepository.BookExistsAsync(bookId, userId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task BookExistsAsync_IdExistButItBelongsToAnotherUser_ReturnFalse()
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
        bool result = await _bookRepository.BookExistsAsync(book.Id, userId: Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task BookExistsAsync_IdExistAndtBelongsToUser_ReturnTrue()
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
        bool result = await _bookRepository.BookExistsAsync(book.Id, book.UserId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteBookAsync_WithExistentBook_ShouldNotExistAfterDelete()
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
        await _bookRepository.DeleteBookAsync(book.Id);

        // Assert
        using IDbConnection connection = DbConnectionFactory.GetConnection();

        const string query = "SELECT COUNT(*) FROM Books WHERE Id = @id";

        int count = await connection.QueryFirstOrDefaultAsync<int>(query, new { book.Id });

        count.Should().Be(0);
    }

    [Fact]
    public async Task GetBookAsync_WithInexistentBook_ReturnNull()
    {
        // Arrange
        var bookId = Guid.NewGuid();

        // Act
        Book.Domain.Entities.Book? book = await _bookRepository.GetBookAsync(bookId);

        // Assert
        book.Should().BeNull();
    }

    [Fact]
    public async Task GetBookAsync_WithExistentBook_ReturnNull()
    {
        var book = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid(),
            title: "Book title",
            author: "Book author",
            genre: "Book genre");

        await _bookRepository.AddBookAsync(book);

        // Act
        Book.Domain.Entities.Book? foundedBook = await _bookRepository.GetBookAsync(book.Id);

        // Assert
        foundedBook.Should().NotBeNull();
        foundedBook?.UserId.Should().Be(book.UserId);
        foundedBook?.Title.Should().Be(book.Title);
        foundedBook?.Author.Should().Be(book.Author);
        foundedBook?.Genre.Should().Be(book.Genre);
    }

    [Fact]
    public async Task UpdateBookAsync_WithExistentBook_UpdateValues()
    {
        var book = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid(),
            title: "Book title",
            author: "Book author",
            genre: "Book genre");

        await _bookRepository.AddBookAsync(book);

        book.Update("New Title", "New Author", "New Genre");

        // Act
        await _bookRepository.UpdateBookAsync(book);

        // Assert
        using IDbConnection connection = DbConnectionFactory.GetConnection();

        const string query = "SELECT Id, UserId, Title, Author, Genre FROM Books WHERE Id = @id";

        Book.Domain.Entities.Book foundedBook = await connection
            .QueryFirstOrDefaultAsync<Book.Domain.Entities.Book>(query, new { book.Id });

        foundedBook.Should().NotBeNull();
        foundedBook.UserId.Should().Be(book.UserId);
        foundedBook.Title.Should().Be("New Title");
        foundedBook.Author.Should().Be("New Author");
        foundedBook.Genre.Should().Be("New Genre");
    }

    [Fact]
    public async Task GetBooksByUserAsync_WithExistentBooks_ReturnBooks()
    {
        var userId = Guid.NewGuid();

        var bookOne = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId,
            title: "Book title one",
            author: "Book author one",
            genre: "Book genre one");

        var bookTwo = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId,
            title: "Book title two",
            author: "Book author two",
            genre: "Book genre two");

        await _bookRepository.AddBookAsync(bookOne);
        await _bookRepository.AddBookAsync(bookTwo);

        // Act
        List<BookDto> books = await _bookRepository.GetBooksByUserAsync(userId);

        // Assert
        var expectedBooks = new List<Book.Domain.Entities.Book> { bookOne, bookTwo }
            .Select(b => new BookDto(b.Id, b.Title, b.Author, b.Genre))
            .OrderBy(b => b.Title)
            .ToList();

        books.Should().BeEquivalentTo(expectedBooks);
    }

    [Fact]
    public async Task GetBookDetailsAsync_WithInexistentBook_ReturnNull()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        BookDto book = await _bookRepository.GetBookDetailsAsync(bookId, userId);

        // Assert
        book.Should().BeNull();
    }

    [Fact]
    public async Task GetBookDetailsAsync_WithExistentBook_ReturnNull()
    {
        var book = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid(),
            title: "Book title",
            author: "Book author",
            genre: "Book genre");

        await _bookRepository.AddBookAsync(book);

        // Act
        BookDto? foundedBook = await _bookRepository.GetBookDetailsAsync(book.Id, book.UserId);

        // Assert
        foundedBook.Should().NotBeNull();
        foundedBook?.Title.Should().Be(book.Title);
        foundedBook?.Author.Should().Be(book.Author);
        foundedBook?.Genre.Should().Be(book.Genre);
    }
}