using Cinema_System.DTOs;
using FluentValidation;

namespace Cinema_System.Validators
{
    public class TicketValidator : AbstractValidator<TicketDTO>
    {
        public TicketValidator()
        {
            RuleFor(t => t.UserName)
                .NotEmpty().WithMessage("User name is required")
                .Matches(@"^[A-Za-z\s_]+$").WithMessage("User name must contain only letters and spaces")
                .MaximumLength(100).WithMessage("User name cannot exceed 100 characters");

            RuleFor(t => t.SessionDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Session date cannot be in the past");

            RuleFor(t => t.SessionStartTime)
                .GreaterThan(TimeSpan.Zero).WithMessage("Session start time must be a positive value");

            RuleFor(t => t.HallName)
                .NotEmpty().WithMessage("Hall name is required")
                .Matches(@"^[A-Za-z\s]+$").WithMessage("Hall name must contain only letters and spaces")
                .MaximumLength(50).WithMessage("Hall name cannot exceed 50 characters");

            RuleFor(t => t.SeatRow)
                .GreaterThan(0).WithMessage("Seat row must be greater than zero");

            RuleFor(t => t.TicketPrice)
                .NotNull().WithMessage("Ticket price is required")
                .GreaterThan(0).WithMessage("Ticket price must be greater than zero");

            RuleFor(t => t.PaymentAmount)
                .GreaterThan(0).WithMessage("Payment amount must be greater than zero");
        }
    }
}
