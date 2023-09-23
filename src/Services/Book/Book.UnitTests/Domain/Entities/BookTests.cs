namespace Book.UnitTests.Domain.Entities;

public class BookTests
{
    [Fact]
    public void Constructor_DefaultId_ShouldThrowArgumentException()
    {
        // Arrange
        Guid id = Guid.Empty;
        var userId = Guid.NewGuid();
        string title = "Book title";
        string author = "Book author";
        string genre = "Book genre";

        // Act
        Action action = () => new Book.Domain.Entities.Book(id, userId, title, author, genre);

        // Asssert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Parameter [id] is default value for type Guid (Parameter 'id')");
    }

    [Fact]
    public void Constructor_DefaultUserId_ShouldThrowArgumentException()
    {
        // Arrange
        var id = Guid.NewGuid();
        Guid userId = Guid.Empty;
        string title = "Book title";
        string author = "Book author";
        string genre = "Book genre";

        // Act
        Action action = () => new Book.Domain.Entities.Book(id, userId, title, author, genre);

        // Asssert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Parameter [userId] is default value for type Guid (Parameter 'userId')");
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Constructor_EmptyTitle_ShouldThrowArgumentException(string title)
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        string author = "Book author";
        string genre = "Book genre";

        // Act
        Action action = () => new Book.Domain.Entities.Book(id, userId, title, author, genre);

        // Asssert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Required input title was empty. (Parameter 'title')");
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Constructor_EmptyAuthor_ShouldThrowArgumentException(string author)
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        string title = "Book title";
        string genre = "Book genre";

        // Act
        Action action = () => new Book.Domain.Entities.Book(id, userId, title, author, genre);

        // Asssert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Required input author was empty. (Parameter 'author')");
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Constructor_EmptyGenre_ShouldThrowArgumentException(string genre)
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        string title = "Book title";
        string author = "Book author";

        // Act
        Action action = () => new Book.Domain.Entities.Book(id, userId, title, author, genre);

        // Asssert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Required input genre was empty. (Parameter 'genre')");
    }

    [Fact]
    public void Constructor_ValidBookData_CreateNewObjectCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        string title = "The Silent Patient";
        string author = "Alex Michaelides";
        string genre = "Psychological Thriller";

        // Act
        var book = new Book.Domain.Entities.Book(id, userId, title, author, genre);

        // Asssert
        book.Id.Should().Be(id);
        book.UserId.Should().Be(userId);
        book.Title.Should().Be(title);
        book.Author.Should().Be(author);
        book.Genre.Should().Be(genre);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Update_EmptyTitle_ShouldThrowArgumentException(string title)
    {
        // Arrange
        var book = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid(),
            title: "Book title",
            author: "Book author",
            genre: "Book genre");

        // Act
        Action action = () => book.Update(title, book.Author, book.Genre);

        // Asssert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Required input title was empty. (Parameter 'title')");
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Update_EmptyAuthor_ShouldThrowArgumentException(string author)
    {
        // Arrange
        var book = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid(),
            title: "Book title",
            author: "Book author",
            genre: "Book genre");

        // Act
        Action action = () => book.Update(book.Title, author, book.Genre);

        // Asssert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Required input author was empty. (Parameter 'author')");
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Update_EmptyGenre_ShouldThrowArgumentException(string genre)
    {
        // Arrange
        var book = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid(),
            title: "Book title",
            author: "Book author",
            genre: "Book genre");

        // Act
        Action action = () => book.Update(book.Title, book.Author, genre);

        // Asssert
        action.Should().Throw<ArgumentException>()
            .WithMessage("Required input genre was empty. (Parameter 'genre')");
    }

    [Fact]
    public void Update_ValidBookData_UpdateBookInformationCorrectly()
    {
        // Arrange
        var book = new Book.Domain.Entities.Book(
            id: Guid.NewGuid(),
            userId: Guid.NewGuid(),
            title: "Book title",
            author: "Book author",
            genre: "Book genre");

        // Act
        book.Update(title: "New book title", author: "Another author", genre: "New genre");

        // Asssert
        book.Id.Should().Be(book.Id);
        book.UserId.Should().Be(book.UserId);
        book.Title.Should().Be("New book title");
        book.Author.Should().Be("Another author");
        book.Genre.Should().Be("New genre");
    }
}
