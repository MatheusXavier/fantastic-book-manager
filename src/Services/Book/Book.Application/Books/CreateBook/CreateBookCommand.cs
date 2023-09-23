using Book.Domain.Common;

using FluentValidation.Results;

namespace Book.Application.Books.CreateBook;

public record CreateBookCommand : BaseCommand
{
    public string Title { get; }

    public string Author { get; }

    public string Genre { get; }

    public CreateBookCommand(string title, string author, string genre)
    {
        Title = title;
        Author = author;
        Genre = genre;
    }

    public override ValidationResult Validate()
    {
        return new CreateBookCommandValidator().Validate(this);
    }
}