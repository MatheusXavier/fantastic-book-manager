using Book.Application.Interfaces;
using Book.Domain.Results;

using MediatR;

using Microsoft.AspNetCore.Http;

namespace Book.Application.Books.CreateBook;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IErrorHandler _errorHandler;

    public CreateBookCommandHandler(
        IBookRepository bookRepository,
        ILoggedUser loggedUser,
        IErrorHandler errorHandler)
    {
        _bookRepository = bookRepository;
        _loggedUser = loggedUser;
        _errorHandler = errorHandler;
    }

    public async Task Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        if (!_errorHandler.ValidateCommand(request))
        {
            return;
        }

        if (await UserAlreadyHasBookTitle(request.Title))
        {
            var errorDetail = new ErrorDetail(
                StatusCodes.Status400BadRequest,
                new ErrorMessage("useralreadyhasbooktitle", "User has already registered a book with this title"));

            _errorHandler.Add(errorDetail);
            return;
        }

        var book = new Domain.Entities.Book(request.Id, request.Title, request.Author, request.Genre);

        await _bookRepository.AddBookAsync(book);
    }

    private async Task<bool> UserAlreadyHasBookTitle(string bookTitle)
    {
        int bookTitleCount = await _bookRepository.GetBooksCountByTitleAsync(bookTitle, _loggedUser.Id);

        return bookTitleCount > 0;
    }
}
