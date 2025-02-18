using Cinema_System.DTOs;
using FluentValidation;

namespace Cinema_System.Validators
{
    public class GenreValidator : AbstractValidator<GenreDTO>
    {
        public GenreValidator()
        {
            RuleFor(g => g.GenreName)
                .NotEmpty().WithMessage("Genre name is required.")
                .Matches(@"^[A-Za-z\s]+$").WithMessage("Genre name must contain only letters and spaces.")
                .MaximumLength(50).WithMessage("Genre name cannot exceed 50 characters.");
        }
    }
}