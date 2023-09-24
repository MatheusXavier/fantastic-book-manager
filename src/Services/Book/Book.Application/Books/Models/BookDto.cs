namespace Book.Application.Books.Models;

public record BookDto(Guid Id, string Title, string Author, string Genre);