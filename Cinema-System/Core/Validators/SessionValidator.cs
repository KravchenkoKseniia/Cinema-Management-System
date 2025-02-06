using FluentValidation;
using Cinema_System.DTOs;

namespace Cinema_System.Validators
{
    public class SessionValidator : AbstractValidator<SessionDTO>
    {
        public SessionValidator()
        {

            RuleFor(s => s.MovieTitle)
                .NotEmpty().WithMessage("Movie title is required");

            RuleFor(s => s.StartTime)
                .LessThan(s => s.EndTime).WithMessage("Start time must be before end time");
            
            RuleFor(s => s.EndTime)
                .GreaterThan(s => s.StartTime).WithMessage("End time must be after end time");

            RuleFor(s => s.Date)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Session date cannot be in the past");

            RuleFor(s => s.HallId)
                .NotNull().WithMessage("Hall id is required")
                .GreaterThan(0).WithMessage("Hall ID must be greater than 0");

            RuleFor(s => s.HallName)
                .NotEmpty().WithMessage("Hall name is required");

            RuleFor(s => s.TicketPrice)
                .NotNull().WithMessage("Price is required")
                .GreaterThan(0).WithMessage("Ticket price must be greater than 0");
        }
    }
}