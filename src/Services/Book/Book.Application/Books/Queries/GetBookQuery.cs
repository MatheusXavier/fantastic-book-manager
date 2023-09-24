using Book.Application.Books.Models;
using Book.Application.Interfaces;
using Book.Domain.Common;

using MediatR;

namespace Book.Application.Books.Queries;

public record GetBookQuery(Guid BookId) : BaseQuery<BookDto?>;

public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookDto?>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILoggedUser _loggedUser;

    public GetBookQueryHandler(
        IBookRepository bookRepository,
        ILoggedUser loggedUser)
    {
        _bookRepository = bookRepository;
        _loggedUser = loggedUser;
    }

    public async Task<BookDto?> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        return await _bookRepository.GetBookDetailsAsync(
            request.BookId,
            _loggedUser.GetUserId());
    }
}
