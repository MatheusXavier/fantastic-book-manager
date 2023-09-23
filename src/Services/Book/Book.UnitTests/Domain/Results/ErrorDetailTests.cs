using Book.Domain.Results;

namespace Book.UnitTests.Domain.Results;

public class ErrorDetailTests
{
    [Fact]
    public void AddError_AddingSomeErrorDetails_ShouldFillItCorrectly()
    {
        // Arrange
        int statusCode = 400;
        var errorMessage = new ErrorMessage("errorcode", "This is my error description");
        var errorDetail = new ErrorDetail(statusCode, errorMessage);

        // Act
        errorDetail.AddError(new ErrorItem("errormainreason", "The error was caused by 'X'."));

        // Assert
        errorDetail.Status.Should().Be(400);
        errorDetail.Message.Code.Should().Be("errorcode");
        errorDetail.Message.Description.Should().Be("This is my error description");
        errorDetail.Errors.Should().HaveCount(1);
        errorDetail.Errors.First().Reason.Should().Be("errormainreason");
        errorDetail.Errors.First().Message.Should().Be("The error was caused by 'X'.");
    }
}
