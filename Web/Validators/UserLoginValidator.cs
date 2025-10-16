using FluentValidation;
using MoviesArchive.Web.ViewModels;

namespace MoviesArchive.Web.Validators;

public class UserLoginValidator : AbstractValidator<UserLoginVM>
{
    public UserLoginValidator()
    {
        RuleFor(userRegisterVM => userRegisterVM.Name)
            .NotEmpty().WithMessage("{PropertyName} is not specified")
            .MinimumLength(4).WithMessage("{PropertyName} minimum length is 4 symbols")
            .MaximumLength(16).WithMessage("{PropertyName} maximum length is 16 symbols")
            .Matches(@"^[0-9a-zA-Zа-яА-Я]+$").WithMessage("{PropertyName} must only contain alphanumeric characters.");

        RuleFor(userRegisterVM => userRegisterVM.Password)
            .NotEmpty().WithMessage("{PropertyName} is not specified")
            .MaximumLength(32).WithMessage("{PropertyName} maximum length is 32 symbols");
    }
}