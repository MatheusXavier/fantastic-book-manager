using FluentValidation;

namespace Book.Application.Books.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
                .WithMessage("'{PropertyName}' must not be empty.");

        RuleFor(c => c.Title)
            .NotEmpty()
                .WithMessage("'{PropertyName}' must not be empty.")
            .MaximumLength(150)
                .WithMessage("The length of '{PropertyName}' must be {MaxLength} characters or fewer. You entered {TotalLength} characters.");

        RuleFor(c => c.Author)
            .NotEmpty()
                .WithMessage("'{PropertyName}' must not be empty.")
            .MaximumLength(100)
                .WithMessage("The length of '{PropertyName}' must be {MaxLength} characters or fewer. You entered {TotalLength} characters.");

        RuleFor(c => c.Genre)
            .NotEmpty()
                .WithMessage("'{PropertyName}' must not be empty.")
            .MaximumLength(50)
                .WithMessage("The length of '{PropertyName}' must be {MaxLength} characters or fewer. You entered {TotalLength} characters.");
    }
}