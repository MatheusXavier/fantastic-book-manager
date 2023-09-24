using Book.API.Controllers;
using Book.Application.Books.Commands.CreateBook;
using Book.Application.Books.Commands.DeleteBook;
using Book.Application.Books.Commands.UpdateBook;
using Book.Domain.Results;
using Book.UnitTests.Application.Mocks;
using Book.UnitTests.Infrastructure.Mocks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.UnitTests.API.Controllers;

public class BooksControllerTests
{
    private readonly MockMediatorHandler _mediatorHandler;
    private readonly MockErrorHandler _errorHandler;
    private readonly BooksController _bookController;

    public BooksControllerTests()
    {
        _mediatorHandler = new();
        _errorHandler = new();
        _bookController = new(_mediatorHandler.Object, _errorHandler.Object);
    }

    [Fact]
    public async Task CreateBookAsync_GettingSomeError_ReturnError()
    {
        // Arrange
        var command = new CreateBookCommand("book title", "book author", "book genge");

        var errorMessage = new ErrorMessage(
               "internalerror",
               "The server encountered an unexpected condition that prevented it from fulfilling the request");
        var error = new ErrorDetail(StatusCodes.Status500InternalServerError, errorMessage);
        var errorResult = new ErrorResult(error);

        _errorHandler
            .MockHasError(true)
            .MockGetError(errorResult);

        _mediatorHandler
            .MockSend(command);

        // Act
        IActionResult result = await _bookController.CreateBookAsync(command);

        // Assert
        result.Should().BeEquivalentTo(new ObjectResult(errorResult)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        });

        _errorHandler.VerifyAll();
        _mediatorHandler.VerifyAll();
    }

    [Fact]
    public async Task CreateBookAsync_Success_ReturnOk()
    {
        // Arrange
        var command = new CreateBookCommand("book title", "book author", "book genge");

        _errorHandler
            .MockHasError(false);

        _mediatorHandler
            .MockSend(command);

        // Act
        IActionResult result = await _bookController.CreateBookAsync(command);

        // Assert
        result.Should().BeEquivalentTo(new OkResult());

        _errorHandler.VerifyAll();
        _mediatorHandler.VerifyAll();
    }

    [Fact]
    public async Task DeleteBookAsync_GettingSomeError_ReturnError()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var command = new DeleteBookCommand(bookId);

        var errorMessage = new ErrorMessage(
               "internalerror",
               "The server encountered an unexpected condition that prevented it from fulfilling the request");
        var error = new ErrorDetail(StatusCodes.Status500InternalServerError, errorMessage);
        var errorResult = new ErrorResult(error);

        _errorHandler
            .MockHasError(true)
            .MockGetError(errorResult);

        _mediatorHandler
            .MockSend(command);

        // Act
        IActionResult result = await _bookController.DeleteBookAsync(bookId);

        // Assert
        result.Should().BeEquivalentTo(new ObjectResult(errorResult)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        });

        _errorHandler.VerifyAll();
        _mediatorHandler.VerifyAll();
    }

    [Fact]
    public async Task DeleteBookAsync_Success_ReturnOk()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var command = new DeleteBookCommand(bookId);

        _errorHandler
            .MockHasError(false);

        _mediatorHandler
            .MockSend(command);

        // Act
        IActionResult result = await _bookController.DeleteBookAsync(bookId);

        // Assert
        result.Should().BeEquivalentTo(new OkResult());

        _errorHandler.VerifyAll();
        _mediatorHandler.VerifyAll();
    }

    [Fact]
    public async Task UpdateBookAsync_IdFromRouterIsDifferentFromBody_ReturnError()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var command = new UpdateBookCommand(Guid.NewGuid(), "title", "author", "genre");

        var errorMessage = new ErrorMessage("bookidmismatch", "The route and body id are not the same");
        var error = new ErrorDetail(StatusCodes.Status400BadRequest, errorMessage);
        var errorResult = new ErrorResult(error);

        // Act
        IActionResult result = await _bookController.UpdateBookAsync(bookId, command);

        // Assert
        result.Should().BeEquivalentTo(new ObjectResult(errorResult)
        {
            StatusCode = StatusCodes.Status400BadRequest
        });

        _errorHandler.VerifyAll();
        _mediatorHandler.VerifyAll();
    }

    [Fact]
    public async Task UpdateBookAsync_GettingSomeError_ReturnError()
    {
        // Arrange
        var command = new UpdateBookCommand(Guid.NewGuid(), "title", "author", "genre");

        _errorHandler
            .MockHasError(false);

        _mediatorHandler
            .MockSend(command);

        // Act
        IActionResult result = await _bookController.UpdateBookAsync(command.Id, command);

        // Assert
        result.Should().BeEquivalentTo(new OkResult());

        _errorHandler.VerifyAll();
        _mediatorHandler.VerifyAll();
    }
}