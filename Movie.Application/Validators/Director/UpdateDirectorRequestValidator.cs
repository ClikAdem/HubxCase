using FluentValidation;
using Movie.Application.Dtos.Director;

namespace Movie.Application.Validators.Director;

public class UpdateDirectorRequestValidator : AbstractValidator<UpdateDirectorRequest>
{
    public UpdateDirectorRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Director ID is required");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required")
            .LessThan(DateTime.UtcNow).WithMessage("Birth date must be in the past")
            .GreaterThan(DateTime.UtcNow.AddYears(-150)).WithMessage("Birth date is not valid");

        RuleFor(x => x.Bio)
            .MaximumLength(2000).WithMessage("Bio cannot exceed 2000 characters");
    }
}