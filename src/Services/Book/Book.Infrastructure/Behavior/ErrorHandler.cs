using Book.Application.Interfaces;
using Book.Domain.Common;
using Book.Domain.Results;

using FluentValidation.Results;

using Microsoft.AspNetCore.Http;

namespace Book.Infrastructure.Behavior;

public class ErrorHandler : IErrorHandler
{
    private ErrorResult? _error;

    public bool ValidateCommand(BaseCommand command)
    {
        ValidationResult validationResult = command.Validate();

        if (validationResult.IsValid)
        {
            return true;
        }

        Add(validationResult);

        return false;
    }

    public void Add(ValidationResult validationResult)
    {
        if (validationResult.IsValid)
        {
            throw new InvalidOperationException("Cannot add validation result error that has no error.");
        }

        var errorMessage = new ErrorMessage("invalidfields", "There are some invalid fields");
        var errorDetail = new ErrorDetail(StatusCodes.Status400BadRequest, errorMessage);

        foreach (ValidationFailure error in validationResult.Errors)
        {
            errorDetail.AddError(new ErrorItem(error.ErrorCode, error.ErrorMessage));
        }

        Add(errorDetail);
    }

    public void Add(ErrorDetail errorDetail) => _error = new ErrorResult(errorDetail);

    public bool HasError() => _error is not null;

    public ErrorResult GetError() => _error
        ?? throw new InvalidOperationException("Cannot get error when there is no error.");
}
