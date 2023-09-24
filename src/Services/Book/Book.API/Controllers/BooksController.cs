using Book.Application.Books.CreateBook;
using Book.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers;

public class BooksController : MainController
{
    private readonly IMediatorHandler _mediatorHandler;

    public BooksController(
        IMediatorHandler mediatorHandler,
        IErrorHandler errorHandler) : base(errorHandler)
    {
        _mediatorHandler = mediatorHandler;
    }

    [HttpPost("api/v1/books")]
    public async Task<IActionResult> CreateBookAsync(CreateBookCommand command)
    {
        await _mediatorHandler.Send(command);

        if (IsSuccess())
        {
            return Ok();
        }

        return Error();
    }
}
