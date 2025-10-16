using FluentValidation;
using MoviesArchive.Web.ViewModels;

namespace MoviesArchive.Web.Validators;

public class UserRegisterValidator : AbstractValidator<UserRegisterVM>
{
    public UserRegisterValidator()
    {
        string[] blacklistedWords = ["admin"];

        When(userRegisterVM => userRegisterVM.Name is not null, () =>
        {
            RuleFor(userRegisterVM => userRegisterVM.Name)
                .Must(pass => !blacklistedWords.Any(word => pass.ToLower().Contains(word.ToLower())))
                    .WithMessage("{PropertyName} contains a prohibited word");
        });

        RuleFor(userRegisterVM => userRegisterVM.Name)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MinimumLength(4).WithMessage("{PropertyName} minimum length is 4 symbols")
            .MaximumLength(16).WithMessage("{PropertyName} maximum length is 16 symbols")
            .Matches(@"^[0-9a-zA-Zа-яА-Я]+$").WithMessage("{PropertyName} must only contain alphanumeric characters");

        RuleFor(userRegisterVM => userRegisterVM.Email)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .EmailAddress().WithMessage("{PropertyName} format is incorrect");

        RuleFor(userRegisterVM => userRegisterVM.Password)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MinimumLength(8).WithMessage("{PropertyName} minimum length is 8 symbols")
            .MaximumLength(32).WithMessage("{PropertyName} maximum length is 32 symbols");

        RuleFor(userRegisterVM => userRegisterVM.PasswordConfirm)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Equal(userRegisterVM => userRegisterVM.Password).WithMessage("Password don't match");
    }
}