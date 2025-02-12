using Cinema_System.DTOs;
using FluentValidation;

namespace Cinema_System.Validators
{
    public class PaymentValidator : AbstractValidator<PaymentDTO>
    {
        public PaymentValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("User name is required.")
                .Matches(@"^[A-Za-z\s_]+$").WithMessage("User name must contain only letters, spaces, and underscores.")
                .MaximumLength(100).WithMessage("User name cannot exceed 100 characters.");

            RuleFor(p => p.Amount)
                .NotNull().WithMessage("Amount is required")
                .GreaterThan(0).WithMessage("Amount must be greater than zero");

            RuleFor(p => p.Date)
                .NotNull().WithMessage("Date is required")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Payment date cannot be in the future");
        }
    }
}