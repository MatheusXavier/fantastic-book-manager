using Book.Application.Interfaces;
using Book.Domain.Results;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers;

[ApiController]
[Authorize]
public abstract class MainController : ControllerBase
{
    private readonly IErrorHandler _errorHandler;

    protected MainController(IErrorHandler errorHandler)
    {
        _errorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
    }

    protected IActionResult Error()
    {
        ErrorResult errorResult = _errorHandler.GetError();

        return new ObjectResult(errorResult)
        {
            StatusCode = errorResult.Error.Status,
        };
    }

    protected bool IsSuccess() => !_errorHandler.HasError();
}
