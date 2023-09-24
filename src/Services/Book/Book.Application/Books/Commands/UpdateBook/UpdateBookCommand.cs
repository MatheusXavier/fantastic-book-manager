using Book.Domain.Common;

using FluentValidation.Results;

namespace Book.Application.Books.Commands.UpdateBook;

public record UpdateBookCommand : BaseCommand
{
    public Guid Id { get; }

    public string Title { get; }

    public string Author { get; }

    public string Genre { get; }

    public UpdateBookCommand(Guid id, string title, string author, string genre)
    {
        Id = id;
        Title = title;
        Author = author;
        Genre = genre;
    }

    public override ValidationResult Validate()
    {
        return new UpdateBookCommandValidator().Validate(this);
    }
}