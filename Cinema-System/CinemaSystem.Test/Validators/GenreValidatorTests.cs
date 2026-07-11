using Cinema_System.DTOs;
using Cinema_System.Validators;

namespace CinemaSystem.Test.Validators
{
    public class GenreValidatorTests
    {
        private readonly GenreValidator _validator;

        public GenreValidatorTests()
        {
            _validator = new GenreValidator();
        }

        [Fact]
        public void GenreName_ShouldFail_WhenEmpty()
        {
            var dto = new GenreDTO { GenreName = "" };
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Genre name is required.");
        }

        [Fact]
        public void GenreName_ShouldFail_WhenHasSpecialCharactersOrNumbers()
        {
            var dto = new GenreDTO { GenreName = "Action123!" };
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Genre name must contain only letters and spaces.");
        }

        [Fact]
        public void GenreName_ShouldPass_WhenValid()
        {
            var dto = new GenreDTO { GenreName = "Sci Fi" };
            var result = _validator.Validate(dto);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void GenreName_ShouldFail_WhenExceeds50Characters()
        {
            var dto = new GenreDTO { GenreName = new string('A', 51) };
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Genre name cannot exceed 50 characters.");
        }
    }
}
