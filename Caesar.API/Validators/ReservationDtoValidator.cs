using Caesar.Core.DTOs;
using FluentValidation;
using System;

namespace Caesar.API.Validators;

public class ReservationDtoValidator : AbstractValidator<ReservationDto>
{
    public ReservationDtoValidator()
    {
        RuleFor(x => x.Date).NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
        RuleFor(x => x.Time).NotEmpty();
        RuleFor(x => x.NumberOfGuests).GreaterThan(0).LessThanOrEqualTo(20);
        RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ContactNumber).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Please enter a valid phone number.");
    }
}
