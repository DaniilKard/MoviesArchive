using FluentValidation;
using MoviesArchive.Web.ViewModels;

namespace MoviesArchive.Web.Validators;

public class GenreValidator : AbstractValidator<GenreVM>
{
    public GenreValidator()
    {
        RuleFor(genreIndex => genreIndex.Name).NotEmpty();
    }
}
 