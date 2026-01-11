using FluentValidation;
using Barbershop.Application.DTOs;

namespace Barbershop.Application.Validators;

public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
{
    private readonly string[] _validBarbers = { "Alex", "Jordan", "Sam", "Taylor" };
    private readonly string[] _validServices = { "Classic Cut", "Beard Trim", "Hot Shave" };
    private readonly string[] _validTimeSlots = {
        "09:00", "09:30", "10:00", "10:30", "11:00", "11:30",
        "12:00", "12:30", "13:00", "13:30", "14:00", "14:30",
        "15:00", "15:30", "16:00", "16:30", "17:00", "17:30",
        "18:00", "18:30", "19:00", "19:30"
    };

    public CreateBookingDtoValidator()
    {
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("Client name is required")
            .MaximumLength(100).WithMessage("Client name must not exceed 100 characters");

        RuleFor(x => x.ClientPhone)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^[\d\s\(\)\-]+$").WithMessage("Invalid phone number format")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required")
            .Must(BeValidDate).WithMessage("Invalid date format")
            .Must(BeFutureOrToday).WithMessage("Date cannot be in the past");

        RuleFor(x => x.Time)
            .NotEmpty().WithMessage("Time is required")
            .Must(time => _validTimeSlots.Contains(time)).WithMessage("Invalid time slot");

        RuleFor(x => x.ServiceType)
            .NotEmpty().WithMessage("Service type is required")
            .Must(service => _validServices.Contains(service)).WithMessage("Invalid service type");

        RuleFor(x => x.BarberName)
            .NotEmpty().WithMessage("Barber name is required")
            .Must(barber => _validBarbers.Contains(barber)).WithMessage("Invalid barber name");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }

    private bool BeValidDate(string date)
    {
        return DateTime.TryParse(date, out _);
    }

    private bool BeFutureOrToday(string date)
    {
        if (!DateTime.TryParse(date, out var parsedDate))
            return false;

        var today = DateTime.Today;
        return parsedDate.Date >= today;
    }
}
