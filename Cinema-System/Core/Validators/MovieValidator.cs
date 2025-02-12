using FluentValidation;
using Cinema_System.DTOs;

namespace Cinema_System.Validators
{
    public class MovieValidator : AbstractValidator<MovieDTO>
    {
        public MovieValidator()
        {
            RuleFor(m => m.Title)
                .NotNull().WithMessage("Title is required")
                .NotEmpty().WithMessage("Title cannot be empty")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

            RuleFor(m => m.Description)
                .NotNull().WithMessage("Description is required")
                .NotEmpty().WithMessage("Description cannot be empty")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long");

            RuleFor(m => m.TrailerURL)
                .NotNull().WithMessage("Trailer URL is required")
                .NotEmpty().WithMessage("Trailer URL cannot be empty")
                .Must(IsUrl).WithMessage("Invalid URL format");

            /*RuleFor(m => m.ReleaseDate)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Release date cannot be in the future");*/

            RuleFor(m => m.Rating)
                .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10");

            RuleFor(m => m.Duration)
                .GreaterThan(TimeSpan.FromMinutes(30)).WithMessage("Duration must be at least 30 minutes");

            RuleFor(m => m.PosterURL)
                .NotNull().WithMessage("Poster URL is required")
                .NotEmpty().WithMessage("Poster URL cannot be empty")
                .Must(IsUrl).WithMessage("Invalid Poster URL format");
        }
        
        private static bool IsUrl(string? link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return false;
            }

            Uri? outUri;
            return Uri.TryCreate(link, UriKind.Absolute, out outUri)
                   && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        } //його код, ай ем нот шур іф ітс окей ту юз
    }
}
