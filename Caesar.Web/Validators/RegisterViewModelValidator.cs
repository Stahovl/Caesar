using Caesar.Web.Models;
using FluentValidation;

namespace Caesar.Web.Validators;

public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterViewModelValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .Length(5, 100).WithMessage("Username must be between 5 and 100 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match.");
    }
}