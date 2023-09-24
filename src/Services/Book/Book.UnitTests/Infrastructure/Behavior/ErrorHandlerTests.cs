using Book.Domain.Common;
using Book.Domain.Results;
using Book.Infrastructure.Behavior;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Http;

namespace Book.UnitTests.Infrastructure.Behavior;

public class ErrorHandlerTests
{
    private readonly ErrorHandler _errorHandler;

    public ErrorHandlerTests()
    {
        _errorHandler = new ErrorHandler();
    }

    [Fact]
    public void ValidateCommand_CommandWithNoValidationErrors_ShouldReturnTrueAndNotAddError()
    {
        // Arrange
        var command = new CreateTestCommand(Guid.NewGuid(), "Some Name");

        // Act
        bool result = _errorHandler.ValidateCommand(command);

        // Assert
        result.Should().BeTrue(because: "Command is valid");

        _errorHandler.HasError().Should().BeFalse(because: "Command is valid");
    }

    [Fact]
    public void ValidateCommand_CommandWithValidationErrors_ShouldReturnFalseAndAddError()
    {
        // Arrange
        var command = new CreateTestCommand(Guid.Empty, string.Empty);

        // Act
        bool result = _errorHandler.ValidateCommand(command);

        // Assert
        result.Should().BeFalse(because: "Command is invalid");

        _errorHandler.HasError().Should().BeTrue(because: "Command is invalid");

        var errorMessage = new ErrorMessage("invalidfields", "There are some invalid fields");
        var errorDetail = new ErrorDetail(StatusCodes.Status400BadRequest, errorMessage);
        errorDetail.AddError(new ErrorItem("NotEmptyValidator", "'Id' must not be empty."));
        errorDetail.AddError(new ErrorItem("NotEmptyValidator", "'Name' must not be empty."));
        var expectedError = new ErrorResult(errorDetail);

        _errorHandler.GetError().Should().BeEquivalentTo(expectedError);
    }

    [Fact]
    public void Add_ValidationResultWithNoErrors_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var validationResult = new ValidationResult();

        // Act
        Action action = () => _errorHandler.Add(validationResult);

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot add validation result error that has no error.");
    }

    [Fact]
    public void Add_ValidationResultWithErrors_ShouldAddCorrectly()
    {
        // Arrange
        var validationResult = new ValidationResult();

        validationResult.Errors.AddRange(new List<ValidationFailure>()
            {
                new ValidationFailure("Id", "'Id' must not be empty.")
                {
                    ErrorCode = "NotEmptyValidator",
                },
                new ValidationFailure("Name", "The length of 'Name' must be 150 characters or fewer. You entered 190 characters.")
                {
                    ErrorCode = "MaximumLengthValidator",
                },
            });

        // Act
        _errorHandler.Add(validationResult);

        // Assert
        var errorMessage = new ErrorMessage("invalidfields", "There are some invalid fields");
        var errorDetail = new ErrorDetail(StatusCodes.Status400BadRequest, errorMessage);
        errorDetail.AddError(new ErrorItem("NotEmptyValidator", "'Id' must not be empty."));
        errorDetail.AddError(new ErrorItem("MaximumLengthValidator", "The length of 'Name' must be 150 characters or fewer. You entered 190 characters."));
        var expectedError = new ErrorResult(errorDetail);

        _errorHandler.HasError().Should().BeTrue();
        _errorHandler.GetError().Should().BeEquivalentTo(expectedError);
    }

    [Fact]
    public void Add_ErrorDetail_ShouldAddCorrectly()
    {
        // Arrange
        int status = StatusCodes.Status400BadRequest;
        var errorMessage = new ErrorMessage("Code", "Description");
        var errorDetail = new ErrorDetail(status, errorMessage);

        // Act
        _errorHandler.Add(errorDetail);

        // Assert
        _errorHandler.HasError().Should().BeTrue();
        _errorHandler.GetError().Should().BeEquivalentTo(new ErrorResult(errorDetail));
    }

    [Fact]
    public void GetError_WithoutError_ShouldThrowInvalidOperationException()
    {
        // Act
        Action action = () => _errorHandler.GetError();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot get error when there is no error.");
    }

    public record CreateTestCommand : BaseCommand
    {
        public CreateTestCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }

        public string Name { get; }

        public override ValidationResult Validate()
        {
            return new CreateTestCommandValidation().Validate(this);
        }
    }

    public class CreateTestCommandValidation : AbstractValidator<CreateTestCommand>
    {
        public CreateTestCommandValidation()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                    .WithMessage("'{PropertyName}' must not be empty.");

            RuleFor(c => c.Name)
                .NotEmpty()
                    .WithMessage("'{PropertyName}' must not be empty.")
                .MaximumLength(150)
                    .WithMessage("The length of '{PropertyName}' must be {MaxLength} characters or fewer. You entered {TotalLength} characters.");
        }
    }
}
