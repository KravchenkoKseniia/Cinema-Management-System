using Cinema_System.DTOs;
using Cinema_System.Validators;

namespace CinemaSystem.Test.Validators;

public class TicketValidatorTests
{
    private readonly TicketValidator _validator;

    public TicketValidatorTests()
    {
        _validator = new TicketValidator();
    }

    [Fact]
    public void Should_Have_Error_When_UserName_Is_Empty()
    {
        var model = new TicketDTO { UserName = "" };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "UserName");
    }

    [Fact]
    public void Should_Have_Error_When_UserName_Contains_Special_Characters()
    {
        var model = new TicketDTO { UserName = "User@Name!" };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "UserName");
    }

    [Fact]
    public void Should_Have_Error_When_SessionDate_Is_In_Past()
    {
        var model = new TicketDTO { SessionDate = DateTime.Today.AddDays(-1) };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "SessionDate");
    }

    [Fact]
    public void Should_Have_Error_When_SessionStartTime_Is_Negative_Or_Zero()
    {
        var model = new TicketDTO { SessionStartTime = TimeSpan.Zero };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "SessionStartTime");
    }

    [Fact]
    public void Should_Have_Error_When_HallName_Is_Empty()
    {
        var model = new TicketDTO { HallName = "" };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "HallName");
    }

    [Fact]
    public void Should_Have_Error_When_SeatRow_Is_Zero_Or_Less()
    {
        var model = new TicketDTO { SeatRow = 0 };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "SeatRow");
    }

    [Fact]
    public void Should_Have_Error_When_TicketPrice_Is_Zero_Or_Less()
    {
        var model = new TicketDTO { TicketPrice = 0 };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "TicketPrice");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new TicketDTO
        {
            UserName = "John Doe",
            SessionDate = DateTime.Today,
            SessionStartTime = TimeSpan.FromHours(14),
            HallName = "Main Hall",
            SeatRow = 5,
            TicketPrice = 12.50m,
            PaymentAmount = 12.50m
        };
        var result = _validator.Validate(model);
        Assert.True(result.IsValid);
    }
}
