using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using MVC.ControllersUI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;

namespace CinemaSystem.Test.Controllers
{
    public class SessionsUIControllerTests
    {
        private readonly Mock<ISessionService> _mockSessionService;
        private readonly Mock<IMovieService> _mockMovieService;
        private readonly Mock<IHallService> _mockHallService;
        private readonly SessionsUIController _controller;

        public SessionsUIControllerTests()
        {
            _mockSessionService = new Mock<ISessionService>();
            _mockMovieService = new Mock<IMovieService>();
            _mockHallService = new Mock<IHallService>();
            _controller = new SessionsUIController(
                _mockSessionService.Object,
                _mockMovieService.Object,
                _mockHallService.Object
            );
        }

        [Fact]
        public void Index_WithNoFilters_ShouldReturnAllSessions()
        {
            // Arrange
            var sessions = new List<SessionDTO> { new SessionDTO { SessionId = 1 } };
            _mockSessionService.Setup(s => s.GetAllSessions()).Returns(sessions);

            // Act
            var result = _controller.Index(null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SessionDTO>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Index_WithMovieFilterAndMovieNotFound_ShouldReturnEmptyList()
        {
            // Arrange
            _mockMovieService.Setup(s => s.GetMovieIdByName("NonExistent")).Returns(0);

            // Act
            var result = _controller.Index("NonExistent", null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SessionDTO>>(viewResult.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void Index_WithMovieFilterAndMovieFound_ShouldReturnFilteredSessions()
        {
            // Arrange
            var sessions = new List<SessionDTO> { new SessionDTO { SessionId = 1, MovieId = 5 } };
            _mockMovieService.Setup(s => s.GetMovieIdByName("Inception")).Returns(5);
            _mockSessionService.Setup(s => s.GetSessionsByMovie(5)).Returns(sessions);

            // Act
            var result = _controller.Index("Inception", null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SessionDTO>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Index_WithMovieAndDateFiltersAndMovieNotFound_ShouldReturnEmptyList()
        {
            // Arrange
            _mockMovieService.Setup(s => s.GetMovieIdByName("NonExistent")).Returns(0);

            // Act
            var result = _controller.Index("NonExistent", DateTime.Today);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SessionDTO>>(viewResult.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void Index_WithMovieAndDateFiltersAndMovieFound_ShouldReturnFilteredSessions()
        {
            // Arrange
            var date = DateTime.Today;
            var sessions = new List<SessionDTO> { new SessionDTO { SessionId = 1, MovieId = 5 } };
            _mockMovieService.Setup(s => s.GetMovieIdByName("Inception")).Returns(5);
            _mockSessionService.Setup(s => s.GetSessionsByMovieAndDate(5, date)).Returns(sessions);

            // Act
            var result = _controller.Index("Inception", date);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SessionDTO>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Details_WhenSessionDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _mockSessionService.Setup(s => s.GetSessionById(1)).Returns((SessionDTO?)null);

            // Act
            var result = _controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Details_WhenSessionExists_ShouldReturnViewWithSession()
        {
            // Arrange
            var session = new SessionDTO { SessionId = 1 };
            _mockSessionService.Setup(s => s.GetSessionById(1)).Returns(session);

            // Act
            var result = _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(session, viewResult.Model);
        }

        [Fact]
        public void Create_GET_ShouldPopulateViewBagAndReturnView()
        {
            // Arrange
            var movies = new List<MovieDTO> { new MovieDTO { MovieId = 1, Title = "Movie" } };
            var halls = new List<HallDTO> { new HallDTO { HallId = 2, Name = "Hall" } };
            _mockMovieService.Setup(s => s.GetAllMovies()).Returns(movies);
            _mockHallService.Setup(s => s.GetAllHalls()).Returns(halls);

            // Act
            var result = _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(_controller.ViewBag.Movies);
            Assert.NotNull(_controller.ViewBag.Halls);
        }

        [Fact]
        public void Create_POST_WithValidModel_ShouldCallCreateAndRedirect()
        {
            // Arrange
            var sessionDto = new SessionDTO { SessionId = 1 };

            // Act
            var result = _controller.Create(sessionDto);

            // Assert
            _mockSessionService.Verify(s => s.CreateSession(sessionDto), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);
        }

        [Fact]
        public void Create_POST_WithInvalidModel_ShouldRepopulateViewBagAndReturnView()
        {
            // Arrange
            var sessionDto = new SessionDTO { SessionId = 1, MovieId = 1, HallId = 2 };
            _controller.ModelState.AddModelError("MovieTitle", "Movie title is required");

            var movies = new List<MovieDTO> { new MovieDTO { MovieId = 1, Title = "Movie" } };
            var halls = new List<HallDTO> { new HallDTO { HallId = 2, Name = "Hall" } };
            _mockMovieService.Setup(s => s.GetAllMovies()).Returns(movies);
            _mockHallService.Setup(s => s.GetAllHalls()).Returns(halls);

            // Act
            var result = _controller.Create(sessionDto);

            // Assert
            _mockSessionService.Verify(s => s.CreateSession(It.IsAny<SessionDTO>()), Times.Never);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(sessionDto, viewResult.Model);
            Assert.NotNull(_controller.ViewBag.Movies);
            Assert.NotNull(_controller.ViewBag.Halls);
        }

        [Fact]
        public void Delete_POST_ShouldCallDeleteAndRedirect()
        {
            // Arrange
            int sessionId = 1;

            // Act
            var result = _controller.Delete(sessionId);

            // Assert
            _mockSessionService.Verify(s => s.DeleteSession(sessionId), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);
        }

        [Fact]
        public void Edit_GET_WhenSessionDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _mockSessionService.Setup(s => s.GetSessionById(1)).Returns((SessionDTO?)null);

            // Act
            var result = _controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_GET_WhenSessionExists_ShouldReturnViewWithMappedSession()
        {
            // Arrange
            var session = new SessionDTO { SessionId = 1, MovieId = 10, HallId = 2 };
            var movie = new MovieDTO { MovieId = 10, Title = "Inception" };
            var hall = new HallDTO { HallId = 2, Name = "Hall A" };

            _mockSessionService.Setup(s => s.GetSessionById(1)).Returns(session);
            _mockMovieService.Setup(s => s.GetMovieById(10)).Returns(movie);
            _mockHallService.Setup(s => s.GetHallById(2)).Returns(hall);

            // Act
            var result = _controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedDto = Assert.IsType<SessionDTO>(viewResult.Model);
            Assert.Equal(1, returnedDto.SessionId);
            Assert.Equal("Inception", returnedDto.MovieTitle);
            Assert.Equal("Hall A", returnedDto.HallName);
        }

        [Fact]
        public void Edit_POST_ShouldCallUpdateAndRedirect()
        {
            // Arrange
            var sessionDto = new SessionDTO { SessionId = 1 };

            // Act
            var result = _controller.Edit(sessionDto);

            // Assert
            _mockSessionService.Verify(s => s.UpdateSession(sessionDto), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);
        }
    }
}
