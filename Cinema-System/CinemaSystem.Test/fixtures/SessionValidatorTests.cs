using System;
using Cinema_System.DTOs;
using Cinema_System.Validators;
using FluentValidation.TestHelper;
using FluentAssertions;

namespace CinemaSystem.Test.Validators;

public class SessionValidatorTests
{
    private readonly SessionValidator _validator;

    public SessionValidatorTests()
    {
        _validator = new SessionValidator();
    }

    [Fact]
    public void Validate_ValidSessionDto_PassesValidation()
    {
        // Arrange
        var dto = new SessionDTO
        {
            MovieTitle = "Inception",
            StartTime = new TimeSpan(14, 0, 0),
            EndTime = new TimeSpan(16, 0, 0),
            Date = DateTime.Today,
            HallId = 1,
            HallName = "Main Hall",
            TicketPrice = 10.0m
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyMovieTitle_FailsWithMovieTitleRequiredMessage()
    {
        // Arrange
        var dto = new SessionDTO { MovieTitle = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.MovieTitle)
            .WithErrorMessage("Movie title is required");
    }

    [Fact]
    public void Validate_StartTimeAfterEndTime_FailsWithStartTimeMessage()
    {
        // Arrange
        var dto = new SessionDTO
        {
            StartTime = new TimeSpan(16, 0, 0),
            EndTime = new TimeSpan(14, 0, 0)
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.StartTime)
            .WithErrorMessage("Start time must be before end time");
    }

    [Fact]
    public void Validate_EndTimeBeforeStartTime_FailsWithEndTimeMessageTypo()
    {
        // SMELL: Typo in the message: "End time must be after end time" instead of "after start time".
        
        // Arrange
        var dto = new SessionDTO
        {
            StartTime = new TimeSpan(16, 0, 0),
            EndTime = new TimeSpan(14, 0, 0)
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.EndTime)
            .WithErrorMessage("End time must be after end time");
    }

    [Fact]
    public void Validate_DateInPast_FailsWithDateCannotBeInPastMessage()
    {
        // Arrange
        // SMELL: DateTime.Today is captured at validator instantiation, making it static and prone to timezone/stale date bugs if registered as a singleton.
        var dto = new SessionDTO
        {
            Date = DateTime.Today.AddDays(-1)
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Date)
            .WithErrorMessage("Session date cannot be in the past");
    }

    [Fact]
    public void Validate_HallIdIsZero_FailsWithHallIdMessage()
    {
        // Arrange
        var dto = new SessionDTO { HallId = 0 };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.HallId)
            .WithErrorMessage("Hall ID must be greater than 0");
    }

    [Fact]
    public void Validate_EmptyHallName_FailsWithHallNameRequiredMessage()
    {
        // Arrange
        var dto = new SessionDTO { HallName = "" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.HallName)
            .WithErrorMessage("Hall name is required");
    }

    [Fact]
    public void Validate_HallNameWithSpecialCharacters_FailsWithRegexMessage()
    {
        // Arrange
        var dto = new SessionDTO { HallName = "Hall-A" };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.HallName)
            .WithErrorMessage("Hall name must contain only letters and spaces.");
    }

    [Fact]
    public void Validate_TicketPriceIsZero_FailsWithTicketPriceMessage()
    {
        // Arrange
        var dto = new SessionDTO { TicketPrice = 0 };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.TicketPrice)
            .WithErrorMessage("Ticket price must be greater than 0");
    }
}
