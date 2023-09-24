using Book.Application.Books.Commands.UpdateBook;
using Book.Domain.Results;
using Book.UnitTests.Application.Mocks;

using Microsoft.AspNetCore.Http;

namespace Book.UnitTests.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandHandlerTests
{
    private readonly MockBookRepository _bookRepository;
    private readonly MockLoggedUser _loggedUser;
    private readonly MockErrorHandler _errorHandler;
    private readonly UpdateBookCommandHandler _handler;

    public UpdateBookCommandHandlerTests()
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
        var command = new UpdateBookCommand(
            id: Guid.Empty,
            title: "",
            author: "",
            genre: "");

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
    public async void Handle_BookDoesNotExist_AddError()
    {
        // Arrange
        var command = new UpdateBookCommand(
            Guid.NewGuid(),
            "book title",
            "book author",
            "book genre");
        var errorDetail = new ErrorDetail(
            StatusCodes.Status404NotFound,
            new ErrorMessage("booknotfound", "Informed book does not exists"));

        _errorHandler
            .MockValidateCommand(command, isValid: true)
            .MockAdd(errorDetail);

        _bookRepository
            .MockGetBookAsync(command.Id, book: null);

        // Act
        await _handler.Handle(command, new CancellationToken());

        // Assert
        _bookRepository.VerifyAll();
        _loggedUser.VerifyAll();
        _errorHandler.VerifyAll();
    }

    [Fact]
    public async void Handle_BookDoesNotBelongToUser_AddError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateBookCommand(
            Guid.NewGuid(),
            "book title",
            "book author",
            "book genre");

        var book = new Book.Domain.Entities.Book(
            command.Id,
            userId: Guid.NewGuid(),
            command.Title,
            command.Author,
            command.Genre);

        var errorDetail = new ErrorDetail(
            StatusCodes.Status404NotFound,
            new ErrorMessage("booknotfound", "Informed book does not exists"));

        _errorHandler
            .MockValidateCommand(command, isValid: true)
            .MockAdd(errorDetail);

        _bookRepository
            .MockGetBookAsync(command.Id, book);

        _loggedUser
            .MockGetUserId(userId);

        // Act
        await _handler.Handle(command, new CancellationToken());

        // Assert
        _bookRepository.VerifyAll();
        _loggedUser.VerifyAll();
        _errorHandler.VerifyAll();
    }

    [Fact]
    public async void Handle_BookExists_UpdateIt()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UpdateBookCommand(
            Guid.NewGuid(),
            "book title",
            "book author",
            "book genre");

        var book = new Book.Domain.Entities.Book(
            command.Id,
            userId,
            command.Title,
            command.Author,
            command.Genre);

        _errorHandler
            .MockValidateCommand(command, isValid: true);

        _bookRepository
            .MockGetBookAsync(command.Id, book)
            .MockUpdateBookAsync(book);

        _loggedUser
            .MockGetUserId(userId);

        // Act
        await _handler.Handle(command, new CancellationToken());

        // Assert
        _bookRepository.VerifyAll();
        _loggedUser.VerifyAll();
        _errorHandler.VerifyAll();
    }
}
