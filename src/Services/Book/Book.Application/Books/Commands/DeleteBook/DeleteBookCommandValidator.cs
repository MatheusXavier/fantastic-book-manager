using FluentValidation;

namespace Book.Application.Books.Commands.DeleteBook;

public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookCommandValidator() { }
}
