using Book.Domain.Common;

using FluentValidation.Results;

namespace Book.Application.Books.Commands.DeleteBook;

public record DeleteBookCommand : BaseCommand
{
    public Guid Id { get; set; }

    public DeleteBookCommand(Guid id)
    {
        Id = id;
    }

    public override ValidationResult Validate()
    {
        return new DeleteBookCommandValidator().Validate(this);
    }
}