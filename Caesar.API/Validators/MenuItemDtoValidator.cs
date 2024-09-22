using Caesar.Core.DTOs;
using FluentValidation;

namespace Caesar.API.Validators;

public class MenuItemDtoValidator : AbstractValidator<MenuItemDto>
{
    public MenuItemDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Price).GreaterThan(0).LessThanOrEqualTo(1000);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
    }
}
