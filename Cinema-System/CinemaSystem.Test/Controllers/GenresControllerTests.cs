using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema_System.Services.Interfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.Controllers;
using Xunit;

namespace CinemaSystem.Test.Controllers
{
    public class GenresControllerTests
    {
        private readonly Mock<IGenreService> _mockGenreService;
        private readonly GenresController _controller;

        public GenresControllerTests()
        {
            _mockGenreService = new Mock<IGenreService>();
            _controller = new GenresController(_mockGenreService.Object);
        }

        [Fact]
        public async Task GetGenres_ShouldReturnOk_WithListOfGenres()
        {
            // Arrange
            var genres = new List<Genre>
            {
                new Genre { GenreId = 1, Id = 1, GenreName = "Action" },
                new Genre { GenreId = 2, Id = 2, GenreName = "Drama" }
            };
            _mockGenreService.Setup(s => s.GetGenresAsync()).ReturnsAsync(genres);

            // Act
            var result = await _controller.GetGenres();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGenres = Assert.IsAssignableFrom<List<Genre>>(okResult.Value);
            Assert.Equal(2, returnedGenres.Count);
        }

        [Fact]
        public async Task GetGenreById_ShouldReturnOk_WhenGenreExists()
        {
            // Arrange
            var genre = new Genre { GenreId = 1, Id = 1, GenreName = "Action" };
            _mockGenreService.Setup(s => s.GetGenreByIdAsync(1)).ReturnsAsync(genre);

            // Act
            var result = await _controller.GetGenreById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedGenre = Assert.IsType<Genre>(okResult.Value);
            Assert.Equal("Action", returnedGenre.GenreName);
        }

        [Fact]
        public async Task GetGenreById_ShouldReturnNotFound_WhenGenreDoesNotExist()
        {
            // Arrange
            _mockGenreService.Setup(s => s.GetGenreByIdAsync(99)).ReturnsAsync((Genre?)null);

            // Act
            var result = await _controller.GetGenreById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddGenre_ShouldReturnBadRequest_WhenGenreIsNull()
        {
            // Act
            var result = await _controller.AddGenre(null!);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid genre data", badRequestResult.Value);
        }

        [Fact]
        public async Task AddGenre_ShouldReturnCreatedAtAction_WhenGenreIsValid()
        {
            // Arrange
            var genre = new Genre { GenreId = 3, Id = 3, GenreName = "Comedy" };
            _mockGenreService.Setup(s => s.AddGenreAsync(genre)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddGenre(genre);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(GenresController.GetGenreById), createdResult.ActionName);
            Assert.Equal(3, createdResult.RouteValues?["id"]);
            Assert.Equal(genre, createdResult.Value);
            _mockGenreService.Verify(s => s.AddGenreAsync(genre), Times.Once);
        }
    }
}
