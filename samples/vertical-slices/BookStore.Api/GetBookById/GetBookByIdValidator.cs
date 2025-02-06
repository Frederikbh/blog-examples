using FluentValidation;

namespace BookStore.Api.GetBookById;

public class GetBookByIdValidator : AbstractValidator<GetBookByIdRequest>
{
    public GetBookByIdValidator()
    {
        RuleFor(e => e.Id)
            .NotEmpty().WithMessage("Book ID cannot be empty.")
            .Matches("^\\d+$").WithMessage("Book ID must be a numeric value.");
    }
}
