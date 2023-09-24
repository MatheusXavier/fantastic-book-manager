using Book.Application.Books.DeleteBook;
using Book.Domain.Results;
using Book.UnitTests.Application.Mocks;

using Microsoft.AspNetCore.Http;

namespace Book.UnitTests.Application.Books.DeleteBook;

public class DeleteBookCommandHandlerTests
{
    private readonly MockBookRepository _bookRepository;
    private readonly MockLoggedUser _loggedUser;
    private readonly MockErrorHandler _errorHandler;
    private readonly DeleteBookCommandHandler _handler;

    public DeleteBookCommandHandlerTests()
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
        var command = new DeleteBookCommand(Guid.NewGuid());

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
    public async void Handle_BookDoestNotExist_AddError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new DeleteBookCommand(Guid.NewGuid());
        var errorDetail = new ErrorDetail(
            StatusCodes.Status404NotFound,
            new ErrorMessage("booknotfound", "Informed book does not exists"));

        _errorHandler
            .MockValidateCommand(command, isValid: true)
            .MockAdd(errorDetail);

        _loggedUser
            .MockGetUserId(userId);

        _bookRepository
            .MockBookExistsAsync(command.Id, userId, result: false);

        // Act
        await _handler.Handle(command, new CancellationToken());

        // Assert
        _bookRepository.VerifyAll();
        _loggedUser.VerifyAll();
        _errorHandler.VerifyAll();
    }

    [Fact]
    public async void Handle_BookExists_DeleteIt()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new DeleteBookCommand(Guid.NewGuid());

        _errorHandler
            .MockValidateCommand(command, isValid: true);

        _loggedUser
            .MockGetUserId(userId);

        _bookRepository
            .MockBookExistsAsync(command.Id, userId, result: true)
            .MockDeleteBookAsync(command.Id);

        // Act
        await _handler.Handle(command, new CancellationToken());

        // Assert
        _bookRepository.VerifyAll();
        _loggedUser.VerifyAll();
        _errorHandler.VerifyAll();
    }
}
