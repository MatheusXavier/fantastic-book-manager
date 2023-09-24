using Book.Application.Books.Commands.UpdateBook;

using FluentValidation.Results;

namespace Book.UnitTests.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandTests
{
    [Fact]
    public void Validate_IdIsEmpty_ShouldBeInvalid()
    {
        // Arrange
        var command = new UpdateBookCommand(Guid.Empty, "book title", "book author", "book genre");

        // Act
        ValidationResult result = command.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new List<ValidationFailure>()
            {
                new ValidationFailure("Id", "'Id' must not be empty.", Guid.Empty)
                {
                    ErrorCode = "NotEmptyValidator"
                },

            }, config => config.Excluding(c => c.FormattedMessagePlaceholderValues));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Validate_TitleIsEmpty_ShouldBeInvalid(string title)
    {
        // Arrange
        var command = new UpdateBookCommand(Guid.NewGuid(), title, "book author", "book genre");

        // Act
        ValidationResult result = command.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new List<ValidationFailure>()
            {
                new ValidationFailure("Title", "'Title' must not be empty.", title)
                {
                    ErrorCode = "NotEmptyValidator"
                },

            }, config => config.Excluding(c => c.FormattedMessagePlaceholderValues));
    }

    [Fact]
    public void Validate_TitleLengthMoreThanAllowed_ShouldBeInvalid()
    {
        // Arrange
        string title = @"adsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfr
            adsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfr
            adsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfr";
        var command = new UpdateBookCommand(Guid.NewGuid(), title, "book author", "book genre");

        // Act
        ValidationResult result = command.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new List<ValidationFailure>()
            {
                new ValidationFailure("Title", "The length of 'Title' must be 150 characters or fewer. You entered 232 characters.", title)
                {
                    ErrorCode = "MaximumLengthValidator"
                },

            }, config => config.Excluding(c => c.FormattedMessagePlaceholderValues));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Validate_AuthorIsEmpty_ShouldBeInvalid(string author)
    {
        // Arrange
        var command = new UpdateBookCommand(Guid.NewGuid(), "book title", author, "book genre");

        // Act
        ValidationResult result = command.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new List<ValidationFailure>()
            {
                new ValidationFailure("Author", "'Author' must not be empty.", author)
                {
                    ErrorCode = "NotEmptyValidator"
                },

            }, config => config.Excluding(c => c.FormattedMessagePlaceholderValues));
    }

    [Fact]
    public void Validate_AuthorLengthMoreThanAllowed_ShouldBeInvalid()
    {
        // Arrange
        string author = @"adsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfr
            adsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfr
            adsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfr";
        var command = new UpdateBookCommand(Guid.NewGuid(), "book title", author, "book genre");

        // Act
        ValidationResult result = command.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new List<ValidationFailure>()
            {
                new ValidationFailure("Author", "The length of 'Author' must be 100 characters or fewer. You entered 232 characters.", author)
                {
                    ErrorCode = "MaximumLengthValidator"
                },

            }, config => config.Excluding(c => c.FormattedMessagePlaceholderValues));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Validate_GenreIsEmpty_ShouldBeInvalid(string genre)
    {
        // Arrange
        var command = new UpdateBookCommand(Guid.NewGuid(), "book title", "book author", genre);

        // Act
        ValidationResult result = command.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new List<ValidationFailure>()
            {
                new ValidationFailure("Genre", "'Genre' must not be empty.", genre)
                {
                    ErrorCode = "NotEmptyValidator"
                },

            }, config => config.Excluding(c => c.FormattedMessagePlaceholderValues));
    }

    [Fact]
    public void Validate_GenreLengthMoreThanAllowed_ShouldBeInvalid()
    {
        // Arrange
        string genre = @"adsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfr
            adsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfr
            adsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfradsedc asrfr";
        var command = new UpdateBookCommand(Guid.NewGuid(), "book title", "book author", genre);

        // Act
        ValidationResult result = command.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().BeEquivalentTo(new List<ValidationFailure>()
            {
                new ValidationFailure("Genre", "The length of 'Genre' must be 50 characters or fewer. You entered 232 characters.", genre)
                {
                    ErrorCode = "MaximumLengthValidator"
                },

            }, config => config.Excluding(c => c.FormattedMessagePlaceholderValues));
    }
}
