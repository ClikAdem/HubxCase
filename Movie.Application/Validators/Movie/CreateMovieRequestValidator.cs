using FluentValidation;
using Movie.Application.Dtos.Movie;

namespace Movie.Application.Validators.Movie;

public class CreateMovieRequestValidator : AbstractValidator<CreateMovieRequest>
{
    public CreateMovieRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.ReleaseDate)
            .NotEmpty().WithMessage("Release date is required")
            .LessThanOrEqualTo(DateTime.UtcNow.AddYears(5)).WithMessage("Release date cannot be more than 5 years in the future");

        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Genre is required")
            .MaximumLength(50).WithMessage("Genre cannot exceed 50 characters");

        RuleFor(x => x.Rating)
            .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10");

        RuleFor(x => x.ImdbId)
            .NotEmpty().WithMessage("IMDb ID is required")
            .MaximumLength(20).WithMessage("IMDb ID cannot exceed 20 characters");
    }
}