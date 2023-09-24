using Book.Application.Books.Commands.CreateBook;
using Book.Application.Books.Commands.DeleteBook;
using Book.Application.Books.Commands.UpdateBook;
using Book.Application.Books.Models;
using Book.Application.Books.Queries;
using Book.Application.Interfaces;
using Book.Domain.Results;

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

    [HttpGet("api/v1/books")]
    public async Task<IActionResult> GetBooksAsync()
    {
        return Ok(await _mediatorHandler.Send(new GetBooksQuery()));
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

    [HttpGet("api/v1/books/{bookId}")]
    public async Task<IActionResult> GetBookDetailsAsync(Guid bookId)
    {
        BookDto? book = await _mediatorHandler.Send(new GetBookQuery(bookId));

        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPut("api/v1/books/{bookId}")]
    public async Task<IActionResult> UpdateBookAsync(Guid bookId, UpdateBookCommand command)
    {
        if (bookId != command.Id)
        {
            return Error(new ErrorDetail(
                StatusCodes.Status400BadRequest,
                new ErrorMessage("bookidmismatch", "The route and body id are not the same")));
        }

        await _mediatorHandler.Send(command);

        if (IsSuccess())
        {
            return Ok();
        }

        return Error();
    }

    [HttpDelete("api/v1/books/{bookId}")]
    public async Task<IActionResult> DeleteBookAsync(Guid bookId)
    {
        await _mediatorHandler.Send(new DeleteBookCommand(bookId));

        if (IsSuccess())
        {
            return Ok();
        }

        return Error();
    }
}
