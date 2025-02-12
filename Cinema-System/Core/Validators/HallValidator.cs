using Cinema_System.DTOs;
using FluentValidation;

namespace Cinema_System.Validators
{
    public class HallValidator : AbstractValidator<HallDTO>
    {
        public HallValidator()
        {
            RuleFor(h => h.Name)
                .NotEmpty().WithMessage("Hall name is required")
                .Matches(@"^[A-Za-z\s]+$").WithMessage("Hall name must contain only letters and spaces")
                .MaximumLength(50).WithMessage("Hall name cannot exceed 50 characters.");

            RuleFor(h => h.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than zero")
                .LessThanOrEqualTo(1000).WithMessage("Capacity cannot exceed 1000 seats");
        }
    }
}