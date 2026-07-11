using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using MVC.ControllersUI;

namespace CinemaSystem.Test.Controllers;

public class MovieUIControllerTests
{
    private readonly Mock<IMovieService> _mockMovieService;
    private readonly Mock<ITmdbService> _mockTmdbService;
    private readonly MovieUIController _controller;

    public MovieUIControllerTests()
    {
        _mockMovieService = new Mock<IMovieService>();
        _mockTmdbService = new Mock<ITmdbService>();
        _controller = new MovieUIController(_mockMovieService.Object, _mockTmdbService.Object);

        // Setup TempData
        var tempDataProvider = new Mock<ITempDataProvider>();
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataProvider.Object);
    }

    [Fact]
    public void TmdbSearch_ShouldReturnView()
    {
        var result = _controller.TmdbSearch();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task TmdbSearchResults_ShouldReturnViewWithSearchResults()
    {
        // Arrange
        var searchResult = new MovieSearchResultDTO();
        _mockTmdbService.Setup(s => s.SearchMoviesByTitleAsync("Avatar")).ReturnsAsync(searchResult);

        // Act
        var result = await _controller.TmdbSearchResults("Avatar");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("TmdbSearchResults", viewResult.ViewName);
        Assert.Equal(searchResult, viewResult.Model);
    }

    [Fact]
    public async Task ImportFromTmdb_ShouldRedirectWithNotFoundMessage_WhenMovieNotFoundOnTmdb()
    {
        // Arrange
        _mockTmdbService.Setup(s => s.FetchMovieFromTmdbByIdAsync(1)).ReturnsAsync((MovieDTO?)null);

        // Act
        var result = await _controller.ImportFromTmdb(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Movie not found on TMDB.", _controller.TempData["Message"]);
    }

    [Fact]
    public async Task ImportFromTmdb_ShouldRedirectWithExistsMessage_WhenMovieAlreadyExistsInDb()
    {
        // Arrange
        var movieDto = new MovieDTO { MovieId = 1, Title = "Avatar" };
        _mockTmdbService.Setup(s => s.FetchMovieFromTmdbByIdAsync(1)).ReturnsAsync(movieDto);
        _mockMovieService.Setup(s => s.GetMovieById(1)).Returns(movieDto);

        // Act
        var result = await _controller.ImportFromTmdb(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Movie 'Avatar' already exists.", _controller.TempData["Message"]);
    }

    [Fact]
    public async Task ImportFromTmdb_ShouldCreateMovieAndRedirect_WhenMovieIsValidAndNew()
    {
        // Arrange
        var movieDto = new MovieDTO { MovieId = 1, Title = "Avatar" };
        _mockTmdbService.Setup(s => s.FetchMovieFromTmdbByIdAsync(1)).ReturnsAsync(movieDto);
        _mockMovieService.Setup(s => s.GetMovieById(1)).Returns((MovieDTO?)null);

        // Act
        var result = await _controller.ImportFromTmdb(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockMovieService.Verify(s => s.CreateMovie(movieDto), Times.Once);
    }

    [Fact]
    public void Index_ShouldReturnViewWithMovies()
    {
        // Arrange
        var movies = new List<MovieDTO>();
        _mockMovieService.Setup(s => s.GetAllMovies()).Returns(movies);

        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(movies, viewResult.Model);
    }

    [Fact]
    public void Details_ShouldReturnNotFound_WhenMovieNotFound()
    {
        // Arrange
        _mockMovieService.Setup(s => s.GetMovieById(1)).Returns((MovieDTO?)null);

        // Act
        var result = _controller.Details(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Details_ShouldReturnViewWithMovie_WhenMovieExists()
    {
        // Arrange
        var movie = new MovieDTO { MovieId = 1 };
        _mockMovieService.Setup(s => s.GetMovieById(1)).Returns(movie);

        // Act
        var result = _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(movie, viewResult.Model);
    }

    [Fact]
    public void Create_Get_ShouldReturnView()
    {
        var result = _controller.Create();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Create_Post_ShouldRedirectToIndex_WhenModelStateIsValid()
    {
        // Arrange
        var movie = new MovieDTO { Title = "Avatar" };

        // Act
        var result = _controller.Create(movie);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockMovieService.Verify(s => s.CreateMovie(movie), Times.Once);
    }

    [Fact]
    public void Create_Post_ShouldReturnViewWithModel_WhenModelStateIsInvalid()
    {
        // Arrange
        var movie = new MovieDTO { Title = "Avatar" };
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = _controller.Create(movie);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(movie, viewResult.Model);
        _mockMovieService.Verify(s => s.CreateMovie(movie), Times.Never);
    }

    [Fact]
    public void Edit_Get_ShouldReturnNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        _mockMovieService.Setup(s => s.GetMovieById(1)).Returns((MovieDTO?)null);

        // Act
        var result = _controller.Edit(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Get_ShouldReturnViewWithMovie_WhenMovieExists()
    {
        // Arrange
        var movie = new MovieDTO { MovieId = 1 };
        _mockMovieService.Setup(s => s.GetMovieById(1)).Returns(movie);

        // Act
        var result = _controller.Edit(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(movie, viewResult.Model);
    }

    [Fact]
    public void Edit_Post_ShouldReturnNotFound_WhenIdDoesNotMatchMovieId()
    {
        // Act
        var result = _controller.Edit(1, new MovieDTO { MovieId = 2 });

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Edit_Post_ShouldRedirectToIndex_WhenModelStateIsValid()
    {
        // Arrange
        var movie = new MovieDTO { MovieId = 1, Title = "Updated" };

        // Act
        var result = _controller.Edit(1, movie);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockMovieService.Verify(s => s.UpdateMovie(movie), Times.Once);
    }

    [Fact]
    public void Edit_Post_ShouldReturnViewWithModel_WhenModelStateIsInvalid()
    {
        // Arrange
        var movie = new MovieDTO { MovieId = 1, Title = "Updated" };
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = _controller.Edit(1, movie);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(movie, viewResult.Model);
        _mockMovieService.Verify(s => s.UpdateMovie(movie), Times.Never);
    }

    [Fact]
    public void Delete_Post_ShouldRedirectToIndex()
    {
        // Act
        var result = _controller.Delete(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockMovieService.Verify(s => s.DeleteMovie(1), Times.Once);
    }
}
