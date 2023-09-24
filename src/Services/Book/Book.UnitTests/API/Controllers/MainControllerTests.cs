using Book.API.Controllers;
using Book.Application.Interfaces;
using Book.Domain.Results;
using Book.UnitTests.Application.Mocks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.UnitTests.API.Controllers;

public class MainControllerTests
{
    private readonly MockErrorHandler _errorHandler;
    private readonly TestController _testController;

    public MainControllerTests()
    {
        _errorHandler = new();
        _testController = new(_errorHandler.Object);
    }

    [Fact]
    public void Constructor_PassingNullParameter_ThrowArgumentNullException()
    {
        // Arrange
        IErrorHandler? errorHandler = null;

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        Action action = () => new TestController(errorHandler);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'errorHandler')");
    }

    [Fact]
    public void IsSuccess_WithoutError_ShouldReturnFalse()
    {
        // Arrange
        _errorHandler.MockHasError(false);

        // Act
        bool result = _testController.IsSuccess();

        // Assert
        result.Should().BeTrue();

        _errorHandler.VerifyAll();
    }

    [Fact]
    public void IsSuccess_WithError_ShouldReturnTrue()
    {
        // Arrange
        _errorHandler.MockHasError(true);

        // Act
        bool result = _testController.IsSuccess();

        // Assert
        result.Should().BeFalse();

        _errorHandler.VerifyAll();
    }

    [Fact]
    public void Error_WithErrorInHandler_ReturnErrorWithCorrectStatusCode()
    {
        // Arrange
        var errorMessage = new ErrorMessage("code", "description");
        var errorDetail = new ErrorDetail(StatusCodes.Status500InternalServerError, errorMessage);

        errorDetail.AddError(new ErrorItem("reason1", "message1"));
        errorDetail.AddError(new ErrorItem("reason2", "message2"));
        errorDetail.AddError(new ErrorItem("reason3", "message3"));

        var errorResult = new ErrorResult(errorDetail);

        _errorHandler.MockGetError(errorResult);

        // Act
        IActionResult result = _testController.Error();

        // Assert
        result.Should().BeEquivalentTo(new ObjectResult(errorResult)
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        });

        _errorHandler.VerifyAll();
    }

    public class TestController : MainController
    {
        public TestController(IErrorHandler errorHandler) : base(errorHandler)
        {
        }

        public new bool IsSuccess() => base.IsSuccess();

        public new IActionResult Error() => base.Error();
    }
}
