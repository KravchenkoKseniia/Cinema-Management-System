using Cinema_System.DTOs;
using FluentValidation;

namespace Cinema_System.Validators
{
    public class RoleValidator : AbstractValidator<RoleDTO>
    {
        public RoleValidator()
        {
            RuleFor(r => r.RoleName)
                .NotEmpty().WithMessage("Role name is required")
                .Matches(@"^[A-Za-z\s]+$").WithMessage("Role name must contain only letters and spaces")
                .MaximumLength(50).WithMessage("Role name cannot exceed 50 characters");
        }
    }
}