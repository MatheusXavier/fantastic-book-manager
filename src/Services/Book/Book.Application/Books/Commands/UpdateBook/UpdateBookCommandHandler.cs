using Book.Application.Interfaces;
using Book.Domain.Results;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace Book.Application.Books.Commands.UpdateBook;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IErrorHandler _errorHandler;

    public UpdateBookCommandHandler(
        IBookRepository bookRepository,
        ILoggedUser loggedUser,
        IErrorHandler errorHandler)
    {
        _bookRepository = bookRepository;
        _loggedUser = loggedUser;
        _errorHandler = errorHandler;
    }

    public async Task Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        if (!_errorHandler.ValidateCommand(request))
        {
            return;
        }

        Domain.Entities.Book? book = await _bookRepository.GetBookAsync(request.Id);

        if (book is null)
        {
            _errorHandler.Add(GetNotFoundBookError());
            return;
        }

        bool bookBelongsToCurrentUser = book.UserId == _loggedUser.GetUserId();

        if (!bookBelongsToCurrentUser)
        {
            _errorHandler.Add(GetNotFoundBookError());
            return;
        }

        book.Update(request.Title, request.Author, request.Genre);

        await _bookRepository.UpdateBookAsync(book);
    }

    private static ErrorDetail GetNotFoundBookError()
    {
        var message = new ErrorMessage("booknotfound", "Informed book does not exists");

        return new ErrorDetail(StatusCodes.Status404NotFound, message);
    }
}
