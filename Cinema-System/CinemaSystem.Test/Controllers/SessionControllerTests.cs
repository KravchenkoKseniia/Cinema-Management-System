using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CinemaSystem.Test.Controllers
{
    public class SessionControllerTests
    {
        private readonly Mock<ISessionService> _mockSessionService;
        private readonly SessionController _controller;

        public SessionControllerTests()
        {
            _mockSessionService = new Mock<ISessionService>();
            _controller = new SessionController(_mockSessionService.Object);
        }

        [Fact]
        public void GetAllSessions_ShouldReturnOkWithSessions()
        {
            // Arrange
            var sessions = new List<SessionDTO>
            {
                new SessionDTO { SessionId = 1, MovieTitle = "Movie 1", HallName = "Hall 1" }
            };
            _mockSessionService.Setup(s => s.GetAllSessions()).Returns(sessions);

            // Act
            var result = _controller.GetAllSessions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSessions = Assert.IsAssignableFrom<IEnumerable<SessionDTO>>(okResult.Value);
            Assert.Single(returnedSessions);
        }

        [Fact]
        public void GetSessionById_WhenSessionExists_ShouldReturnOkWithSession()
        {
            // Arrange
            var session = new SessionDTO { SessionId = 1, MovieTitle = "Movie 1" };
            _mockSessionService.Setup(s => s.GetSessionById(1)).Returns(session);

            // Act
            var result = _controller.GetSessionById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSession = Assert.IsType<SessionDTO>(okResult.Value);
            Assert.Equal(1, returnedSession.SessionId);
        }

        [Fact]
        public void GetSessionById_WhenSessionDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _mockSessionService.Setup(s => s.GetSessionById(1)).Returns((SessionDTO?)null);

            // Act
            var result = _controller.GetSessionById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateSession_ShouldCallServiceAndReturnCreatedAtAction()
        {
            // Arrange
            var sessionDto = new SessionDTO { SessionId = 1, MovieTitle = "Movie 1" };

            // Act
            var result = _controller.CreateSession(sessionDto);

            // Assert
            _mockSessionService.Verify(s => s.CreateSession(sessionDto), Times.Once);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetSessionById), createdResult.ActionName);
            Assert.Equal(1, ((SessionDTO)createdResult.Value!).SessionId);
        }
    }
}
