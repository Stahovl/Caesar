using Caesar.Core.DTOs;
using FluentValidation;
using System;

namespace Caesar.API.Validators;

public class ReservationDtoValidator : AbstractValidator<ReservationDto>
{
    public ReservationDtoValidator()
    {
        RuleFor(x => x.ReservationDate).NotEmpty().GreaterThanOrEqualTo(DateTime.Today);
        RuleFor(x => x.ReservationTime).NotEmpty();
        RuleFor(x => x.NumberOfGuests).GreaterThan(0).LessThanOrEqualTo(20);
    }
}
