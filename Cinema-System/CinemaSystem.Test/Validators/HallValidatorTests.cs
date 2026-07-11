using Cinema_System.DTOs;
using Cinema_System.Validators;

namespace CinemaSystem.Test.Validators
{
    public class HallValidatorTests
    {
        private readonly HallValidator _validator;

        public HallValidatorTests()
        {
            _validator = new HallValidator();
        }

        [Fact]
        public void Name_ShouldFail_WhenEmpty()
        {
            var dto = new HallDTO { Name = "", Capacity = 100 };
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Hall name is required");
        }

        [Fact]
        public void Name_ShouldFail_WhenHasSpecialCharactersOrNumbers()
        {
            var dto = new HallDTO { Name = "Hall 123", Capacity = 100 };
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Hall name must contain only letters and spaces");
        }

        [Fact]
        public void Name_ShouldFail_WhenExceeds50Characters()
        {
            var dto = new HallDTO { Name = new string('A', 51), Capacity = 100 };
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Hall name cannot exceed 50 characters.");
        }

        [Fact]
        public void Capacity_ShouldFail_WhenZeroOrNegative()
        {
            var dtoZero = new HallDTO { Name = "Main Hall", Capacity = 0 };
            var resultZero = _validator.Validate(dtoZero);
            Assert.False(resultZero.IsValid);
            Assert.Contains(resultZero.Errors, e => e.ErrorMessage == "Capacity must be greater than zero");

            var dtoNegative = new HallDTO { Name = "Main Hall", Capacity = -10 };
            var resultNegative = _validator.Validate(dtoNegative);
            Assert.False(resultNegative.IsValid);
            Assert.Contains(resultNegative.Errors, e => e.ErrorMessage == "Capacity must be greater than zero");
        }

        [Fact]
        public void Capacity_ShouldFail_WhenExceeds1000()
        {
            var dto = new HallDTO { Name = "Main Hall", Capacity = 1001 };
            var result = _validator.Validate(dto);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Capacity cannot exceed 1000 seats");
        }

        [Fact]
        public void Hall_ShouldPass_WhenValid()
        {
            var dto = new HallDTO { Name = "IMAX Hall", Capacity = 500 };
            var result = _validator.Validate(dto);
            Assert.True(result.IsValid);
        }
    }
}
