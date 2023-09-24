using Book.API.Controllers;
using Book.Application.Books.CreateBook;
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
}
