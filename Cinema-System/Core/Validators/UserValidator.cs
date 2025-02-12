using Cinema_System.DTOs;
using FluentValidation;

namespace Cinema_System.Validators
{
    public class UserValidator : AbstractValidator<UserDTO>
    {
        public UserValidator()
        {
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("User name is required")
                .Matches(@"^[A-Za-z0-9\s_]+$").WithMessage("User name must contain only letters, numbers, spaces, and underscores")
                .MaximumLength(100).WithMessage("User name cannot exceed 100 characters");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(u => u.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .Matches(@"^[A-Za-z\s]+$").WithMessage("Role name must contain only letters and spaces")
                .MaximumLength(50).WithMessage("Role name cannot exceed 50 characters");
        }
    }
}