using FluentValidation;

namespace Book.Application.Books.DeleteBook;

public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookCommandValidator() { }
}
