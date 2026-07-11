using Cinema_System.DTOs;
using Cinema_System.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CinemaSystem.Test.Validators
{
    public class UserValidatorTests
    {
        private readonly UserValidator _validator;

        public UserValidatorTests()
        {
            _validator = new UserValidator();
        }

        [Fact]
        public void Should_Have_Error_When_UserName_Is_Empty()
        {
            var model = new UserDTO { UserName = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.UserName)
                  .WithErrorMessage("User name is required");
        }

        [Fact]
        public void Should_Have_Error_When_UserName_Has_Special_Characters()
        {
            var model = new UserDTO { UserName = "user@name" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.UserName)
                  .WithErrorMessage("User name must contain only letters, numbers, spaces, and underscores");
        }

        [Fact]
        public void Should_Have_Error_When_UserName_Exceeds_MaxLength()
        {
            var model = new UserDTO { UserName = new string('A', 101) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.UserName)
                  .WithErrorMessage("User name cannot exceed 100 characters");
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty_Or_Invalid()
        {
            var model = new UserDTO { Email = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.Email)
                  .WithErrorMessage("Email is required");

            model.Email = "invalid-email";
            result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.Email)
                  .WithErrorMessage("Invalid email format");
        }

        [Fact]
        public void Should_Have_Error_When_RoleName_Is_Empty_Or_Invalid()
        {
            var model = new UserDTO { RoleName = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.RoleName)
                  .WithErrorMessage("Role name is required.");

            model.RoleName = "Role123";
            result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.RoleName)
                  .WithErrorMessage("Role name must contain only letters and spaces");

            model.RoleName = new string('A', 51);
            result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(u => u.RoleName)
                  .WithErrorMessage("Role name cannot exceed 50 characters");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var model = new UserDTO
            {
                UserName = "Valid User_123",
                Email = "valid@example.com",
                RoleName = "Admin User"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
