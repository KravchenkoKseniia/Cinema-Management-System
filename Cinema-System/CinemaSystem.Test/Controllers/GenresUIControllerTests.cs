using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema_System.Services.Interfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.ControllersUI;
using Xunit;

namespace CinemaSystem.Test.Controllers
{
    public class GenresUIControllerTests
    {
        private readonly Mock<IGenreService> _mockGenreService;
        private readonly GenresUIController _controller;

        public GenresUIControllerTests()
        {
            _mockGenreService = new Mock<IGenreService>();
            _controller = new GenresUIController(_mockGenreService.Object);
        }

        [Fact]
        public async Task Index_ShouldReturnView_WithListOfGenres()
        {
            // Arrange
            var genres = new List<Genre>
            {
                new Genre { GenreId = 1, Id = 1, GenreName = "Action" },
                new Genre { GenreId = 2, Id = 2, GenreName = "Comedy" }
            };
            _mockGenreService.Setup(s => s.GetGenresAsync()).ReturnsAsync(genres);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedGenres = Assert.IsAssignableFrom<List<Genre>>(viewResult.Model);
            Assert.Equal(2, returnedGenres.Count);
        }

        [Fact]
        public async Task Details_ShouldReturnView_WhenGenreExists()
        {
            // Arrange
            var genre = new Genre { GenreId = 1, Id = 1, GenreName = "Action" };
            _mockGenreService.Setup(s => s.GetGenreByIdAsync(1)).ReturnsAsync(genre);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedGenre = Assert.IsType<Genre>(viewResult.Model);
            Assert.Equal("Action", returnedGenre.GenreName);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenGenreDoesNotExist()
        {
            // Arrange
            _mockGenreService.Setup(s => s.GetGenreByIdAsync(99)).ReturnsAsync((Genre?)null);

            // Act
            var result = await _controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Get_ShouldReturnView()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Post_ShouldRedirectToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var genre = new Genre { GenreId = 3, Id = 3, GenreName = "Drama" };
            _mockGenreService.Setup(s => s.AddGenreAsync(genre)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(genre);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(GenresUIController.Index), redirectResult.ActionName);
            _mockGenreService.Verify(s => s.AddGenreAsync(genre), Times.Once);
        }

        [Fact]
        public async Task Create_Post_ShouldReturnViewWithGenre_WhenModelStateIsInvalid()
        {
            // Arrange
            var genre = new Genre { GenreId = 3, Id = 3, GenreName = "" };
            _controller.ModelState.AddModelError("GenreName", "Genre name is required.");

            // Act
            var result = await _controller.Create(genre);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(genre, viewResult.Model);
            _mockGenreService.Verify(s => s.AddGenreAsync(It.IsAny<Genre>()), Times.Never);
        }
    }
}
