using Book.Domain.Common;
using Book.Domain.Results;

using FluentValidation.Results;

namespace Book.Application.Interfaces;

public interface IErrorHandler
{
    bool ValidateCommand(BaseCommand command);

    void Add(ValidationResult validationResult);

    void Add(ErrorDetail errorDetail);

    bool HasError();

    /// <summary>
    /// Calling this method without an error it will throw an <see cref="InvalidOperationException"/>,
    /// so always call the <see cref="HasError()"/> before it.
    /// </summary>
    /// <returns>An <see cref="ErrorResult"/> object.</returns>
    ErrorResult GetError();
}
