using FluentValidation;
using MoviesArchive.Web.ViewModels;

namespace MoviesArchive.Web.Validators;

public class MovieEditValidator : AbstractValidator<MovieEditVM>
{
    public MovieEditValidator()
    {
        RuleFor(movieEditVM => movieEditVM.Title)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(movieEditVM => movieEditVM.ReleaseYear)
            .InclusiveBetween(1890, 3000).When(mcm => mcm.ReleaseYear is not null);

        RuleFor(movieEditVM => movieEditVM.Comment)
            .MaximumLength(256).Unless(mcm => string.IsNullOrWhiteSpace(mcm.Comment));

        RuleFor(movieEditVM => movieEditVM.Rating)
            .InclusiveBetween(0, 100).When(mcm => mcm.Rating is not null);
    }
}
