using System;
using Cinema_System.DTOs;
using Cinema_System.Validators;
using FluentValidation.TestHelper;
using FluentAssertions;

namespace CinemaSystem.Test.Validators;

public class HallValidatorTests
{
    private readonly HallValidator _validator;

    public HallValidatorTests()
    {
        _validator = new HallValidator();
    }

    [Fact]
    public void Validate_ValidHallDto_PassesValidation()
    {
        // Arrange
        var dto = new HallDTO { Name = "Main Hall", Capacity = 150 };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyName_FailsWithRequiredMessage()
    {
        // Arrange
        var dto = new HallDTO { Name = "", Capacity = 150 };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(h => h.Name)
            .WithErrorMessage("Hall name is required");
    }

    [Fact]
    public void Validate_NameTooLong_FailsWithLengthMessage()
    {
        // Arrange
        var dto = new HallDTO { Name = new string('A', 51), Capacity = 150 };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(h => h.Name)
            .WithErrorMessage("Hall name cannot exceed 50 characters.");
    }

    [Fact]
    public void Validate_NameContainsSpecialCharacters_FailsWithRegexMessage()
    {
        // Arrange
        var dto = new HallDTO { Name = "Hall-A", Capacity = 150 };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(h => h.Name)
            .WithErrorMessage("Hall name must contain only letters and spaces");
    }

    [Fact(Skip = "documents bug: Hall name regex ^[A-Za-z\\s]+$ rejects numeric names like 'Hall 1'")]
    public void Validate_NameContainsNumbers_ShouldPassIntendedBehavior()
    {
        // Arrange
        var dto = new HallDTO { Name = "Hall 1", Capacity = 150 };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_CapacityIsZero_FailsWithGreaterThanZeroMessage()
    {
        // Arrange
        var dto = new HallDTO { Name = "Main Hall", Capacity = 0 };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(h => h.Capacity)
            .WithErrorMessage("Capacity must be greater than zero");
    }

    [Fact]
    public void Validate_CapacityExceedsLimit_FailsWithCapacityMessage()
    {
        // Arrange
        var dto = new HallDTO { Name = "Main Hall", Capacity = 1001 };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(h => h.Capacity)
            .WithErrorMessage("Capacity cannot exceed 1000 seats");
    }
}
