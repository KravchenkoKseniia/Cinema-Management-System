using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.Controllers;

namespace CinemaSystem.Test.Controllers;

public class MoviesControllerTests
{
    private readonly Mock<IMovieService> _mockMovieService;
    private readonly MovieController _controller;

    public MoviesControllerTests()
    {
        _mockMovieService = new Mock<IMovieService>();
        _controller = new MovieController(_mockMovieService.Object);
    }

    [Fact]
    public void GetAllMovies_ShouldReturnOkWithMovies()
    {
        // Arrange
        var movies = new List<MovieDTO> { new MovieDTO { MovieId = 1, Title = "Movie 1" } };
        _mockMovieService.Setup(s => s.GetAllMovies()).Returns(movies);

        // Act
        var result = _controller.GetAllMovies();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMovies = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(okResult.Value);
        Assert.Single(returnedMovies);
    }

    [Fact]
    public void GetMovieById_ShouldReturnNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        _mockMovieService.Setup(s => s.GetMovieById(1)).Returns((MovieDTO?)null);

        // Act
        var result = _controller.GetMovieById(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Movie not found", notFoundResult.Value);
    }

    [Fact]
    public void GetMovieById_ShouldReturnOkWithMovie_WhenMovieExists()
    {
        // Arrange
        var movie = new MovieDTO { MovieId = 1, Title = "Movie 1" };
        _mockMovieService.Setup(s => s.GetMovieById(1)).Returns(movie);

        // Act
        var result = _controller.GetMovieById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMovie = Assert.IsType<MovieDTO>(okResult.Value);
        Assert.Equal("Movie 1", returnedMovie.Title);
    }

    [Fact]
    public void CreateMovie_ShouldReturnBadRequest_WhenDtoIsNull()
    {
        // Act
        var result = _controller.CreateMovie(null!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid data", badRequestResult.Value);
    }

    [Fact]
    public void CreateMovie_ShouldReturnCreatedAtAction_WhenDtoIsValid()
    {
        // Arrange
        var dto = new MovieDTO { MovieId = 10, Title = "New Movie" };

        // Act
        var result = _controller.CreateMovie(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetMovieById", createdResult.ActionName);
        Assert.Equal(10, ((dynamic)createdResult.RouteValues!)["id"]);
        Assert.Equal(dto, createdResult.Value);
        _mockMovieService.Verify(s => s.CreateMovie(dto), Times.Once);
    }

    [Fact]
    public void UpdateMovie_ShouldReturnBadRequest_WhenDtoIsNull()
    {
        // Act
        var result = _controller.UpdateMovie(1, null!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid data", badRequestResult.Value);
    }

    [Fact]
    public void UpdateMovie_ShouldReturnBadRequest_WhenIdDoesNotMatchDtoId()
    {
        // Arrange
        var dto = new MovieDTO { MovieId = 2 };

        // Act
        var result = _controller.UpdateMovie(1, dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid data", badRequestResult.Value);
    }

    [Fact]
    public void UpdateMovie_ShouldReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var dto = new MovieDTO { MovieId = 1 };

        // Act
        var result = _controller.UpdateMovie(1, dto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockMovieService.Verify(s => s.UpdateMovie(dto), Times.Once);
    }

    [Fact]
    public void DeleteMovie_ShouldReturnNoContent()
    {
        // Act
        var result = _controller.DeleteMovie(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockMovieService.Verify(s => s.DeleteMovie(1), Times.Once);
    }
}
