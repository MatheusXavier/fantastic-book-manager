using Book.Domain.Common;

using FluentValidation.Results;

namespace Book.Application.Books.Commands.CreateBook;

public record CreateBookCommand : BaseCommand
{
    public Guid Id { get; }

    public string Title { get; }

    public string Author { get; }

    public string Genre { get; }

    public CreateBookCommand(string title, string author, string genre)
    {
        Id = Guid.NewGuid();
        Title = title;
        Author = author;
        Genre = genre;
    }

    public override ValidationResult Validate()
    {
        return new CreateBookCommandValidator().Validate(this);
    }
}