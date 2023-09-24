using Book.Application.Books.Commands.CreateBook;
using Book.Domain.Results;
using Book.UnitTests.Application.Mocks;

using Microsoft.AspNetCore.Http;

namespace Book.UnitTests.Application.Books.Commands.CreateBook;

public class CreateBookCommandHandlerTests
{
    private readonly MockBookRepository _bookRepository;
    private readonly MockLoggedUser _loggedUser;
    private readonly MockErrorHandler _errorHandler;
    private readonly CreateBookCommandHandler _handler;

    public CreateBookCommandHandlerTests()
    {
        _bookRepository = new();
        _loggedUser = new();
        _errorHandler = new();
        _handler = new(
            _bookRepository.Object,
            _loggedUser.Object,
            _errorHandler.Object);
    }

    [Fact]
    public async void Handle_InvalidCommand_DoNothing()
    {
        // Arrange
        var command = new CreateBookCommand("", "", "");

        _errorHandler
            .MockValidateCommand(command, isValid: false);

        // Act
        await _handler.Handle(command, new CancellationToken());

        // Assert
        _bookRepository.VerifyAll();
        _loggedUser.VerifyAll();
        _errorHandler.VerifyAll();
    }

    [Fact]
    public async void Handle_UserAlreadyHasBookTitleRegistered_AddError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateBookCommand("book title", "book author", "book genre");

        var errorMessage = new ErrorMessage(
            Code: "useralreadyhasbooktitle",
            Description: "User has already registered a book with this title");
        var errorDetail = new ErrorDetail(StatusCodes.Status400BadRequest, errorMessage);

        _errorHandler
            .MockValidateCommand(command, isValid: true)
            .MockAdd(errorDetail);

        _loggedUser
            .MockGetUserId(userId);

        _bookRepository
            .MockGetBooksCountByTitleAsync(command.Title, userId, count: 1);

        // Act
        await _handler.Handle(command, new CancellationToken());

        // Assert
        _bookRepository.VerifyAll();
        _loggedUser.VerifyAll();
        _errorHandler.VerifyAll();
    }

    [Fact]
    public async void Handle_EverythingRight_CreateNewBook()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateBookCommand("book title", "book author", "book genre");
        var book = new Book.Domain.Entities.Book(
            command.Id,
            userId,
            command.Title,
            command.Author,
            command.Genre);

        _errorHandler
            .MockValidateCommand(command, isValid: true);

        _loggedUser
            .MockGetUserId(userId);

        _bookRepository
            .MockGetBooksCountByTitleAsync(command.Title, userId, count: 0)
            .MockAddBookAsync(book);

        // Act
        await _handler.Handle(command, new CancellationToken());

        // Assert
        _bookRepository.VerifyAll();
        _loggedUser.VerifyAll();
        _errorHandler.VerifyAll();
    }
}
