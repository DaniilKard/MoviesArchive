using FluentValidation;
using MoviesArchive.Web.ViewModels;

namespace MoviesArchive.Web.Validators;

public class MovieCreateValidator : AbstractValidator<MovieCreateVM>
{
    public MovieCreateValidator()
    {
        RuleFor(movieCreateVM => movieCreateVM.Title)
            .NotEmpty()
            .MaximumLength(128)
            .Must(t => !(t.StartsWith(" ") || t.EndsWith(" ")))
                .WithMessage("Title can't start or end with whitespace");

        RuleFor(movieCreateVM => movieCreateVM.ReleaseYear)
            .InclusiveBetween(1890, 3000).When(mcm => mcm.ReleaseYear is not null);

        RuleFor(movieCreateVM => movieCreateVM.Rating)
            .InclusiveBetween(0, 100).When(mcm => mcm.Rating is not null);

        When(movieCreateVM => movieCreateVM.Comment is not null, () =>
        {
            RuleFor(movieCreateVM => movieCreateVM.Comment)
                .MaximumLength(256)
                .Must(c => !(c.StartsWith(" ") || c.EndsWith(" ")))
                    .WithMessage("Comment can't start or end with whitespace");
        });
    }
}
