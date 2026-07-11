using Cinema_System.DTOs;
using Cinema_System.Validators;

namespace CinemaSystem.Test.Validators;

public class MovieValidatorTests
{
    private readonly MovieValidator _validator;

    public MovieValidatorTests()
    {
        _validator = new MovieValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Null_Or_Empty()
    {
        var model = new MovieDTO { Title = "" };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Exceeds_100_Characters()
    {
        var model = new MovieDTO { Title = new string('a', 101) };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Too_Short()
    {
        var model = new MovieDTO { Description = "short" };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "Description");
    }

    [Fact]
    public void Should_Have_Error_When_TrailerURL_Is_Invalid()
    {
        var model = new MovieDTO { TrailerURL = "invalid-url" };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "TrailerURL");
    }

    [Fact]
    public void Should_Have_Error_When_Rating_Is_Out_Of_Range()
    {
        var model = new MovieDTO { Rating = 11 };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "Rating");
    }

    [Fact]
    public void Should_Have_Error_When_Duration_Is_Less_Than_30_Minutes()
    {
        var model = new MovieDTO { Duration = TimeSpan.FromMinutes(20) };
        var result = _validator.Validate(model);
        Assert.Contains(result.Errors, e => e.PropertyName == "Duration");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new MovieDTO
        {
            Title = "Valid Movie",
            Description = "This is a valid long enough description.",
            TrailerURL = "https://youtube.com/trailer",
            Rating = 8.5m,
            Duration = TimeSpan.FromMinutes(120),
            PosterURL = "https://image.com/poster.jpg"
        };
        var result = _validator.Validate(model);
        Assert.True(result.IsValid);
    }
}
