using Book.Application.Books.Models;
using Book.Application.Interfaces;
using Book.Domain.Common;

using MediatR;

namespace Book.UnitTests.Application.Books.Queries;

public record GetBooksQuery : BaseQuery<List<BookDto>>;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<BookDto>>
{
    private readonly IBookRepository _bookRepository;
    private readonly ILoggedUser _loggedUser;

    public GetBooksQueryHandler(
        IBookRepository bookRepository,
        ILoggedUser loggedUser)
    {
        _bookRepository = bookRepository;
        _loggedUser = loggedUser;
    }

    public async Task<List<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return await _bookRepository.GetBooksByUserAsync(_loggedUser.GetUserId());
    }
}
