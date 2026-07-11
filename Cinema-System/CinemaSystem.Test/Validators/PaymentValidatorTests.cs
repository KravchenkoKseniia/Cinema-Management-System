using Cinema_System.DTOs;
using Cinema_System.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CinemaSystem.Test.Validators
{
    public class PaymentValidatorTests
    {
        private readonly PaymentValidator _validator;

        public PaymentValidatorTests()
        {
            _validator = new PaymentValidator();
        }

        [Fact]
        public void Should_Have_Error_When_UserName_Is_Empty()
        {
            var model = new PaymentDTO { UserName = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.UserName)
                  .WithErrorMessage("User name is required.");
        }

        [Fact]
        public void Should_Have_Error_When_UserName_Contains_Special_Characters()
        {
            var model = new PaymentDTO { UserName = "User@123" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.UserName)
                  .WithErrorMessage("User name must contain only letters, spaces, and underscores.");
        }

        [Fact]
        public void Should_Have_Error_When_UserName_Exceeds_MaxLength()
        {
            var model = new PaymentDTO { UserName = new string('A', 101) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.UserName)
                  .WithErrorMessage("User name cannot exceed 100 characters.");
        }

        [Fact]
        public void Should_Have_Error_When_Amount_Is_Zero_Or_Negative()
        {
            var model = new PaymentDTO { Amount = 0 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.Amount)
                  .WithErrorMessage("Amount must be greater than zero");

            model.Amount = -10;
            result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.Amount)
                  .WithErrorMessage("Amount must be greater than zero");
        }

        [Fact]
        public void Should_Have_Error_When_Date_Is_In_Future()
        {
            var model = new PaymentDTO { Date = DateTime.Today.AddDays(1) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.Date)
                  .WithErrorMessage("Payment date cannot be in the future");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var model = new PaymentDTO
            {
                UserName = "Valid_User Name",
                Amount = 50.50m,
                Date = DateTime.Today
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
