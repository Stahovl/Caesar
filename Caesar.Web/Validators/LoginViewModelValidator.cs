using Caesar.Web.Models;
using FluentValidation;

namespace Caesar.Web.Validators;

public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginViewModelValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}