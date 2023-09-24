using Book.Application.Interfaces;
using Book.Domain.Results;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace Book.Application.Books.DeleteBook;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IErrorHandler _errorHandler;

    public DeleteBookCommandHandler(
        IBookRepository bookRepository,
        ILoggedUser loggedUser,
        IErrorHandler errorHandler)
    {
        _bookRepository = bookRepository;
        _loggedUser = loggedUser;
        _errorHandler = errorHandler;
    }

    public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        if (!_errorHandler.ValidateCommand(request))
        {
            return;
        }

        if (!await _bookRepository.BookExistsAsync(request.Id, _loggedUser.GetUserId()))
        {
            var errorDetail = new ErrorDetail(
                StatusCodes.Status404NotFound,
                new ErrorMessage("booknotfound", "Informed book does not exists"));

            _errorHandler.Add(errorDetail);
            return;
        }

        await _bookRepository.DeleteBookAsync(request.Id);
    }
}