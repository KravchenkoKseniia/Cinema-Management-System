using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.Controllers;
using Xunit;

namespace CinemaSystem.Test.Controllers
{
    public class AuthorizationControllerTests
    {
        private readonly Mock<IAuthenticationService> _authServiceMock;
        private readonly AuthorizationController _controller;

        public AuthorizationControllerTests()
        {
            _authServiceMock = new Mock<IAuthenticationService>();
            _controller = new AuthorizationController(_authServiceMock.Object);
        }

        [Fact]
        public void Register_ShouldReturnBadRequest_WhenRegistrationFails()
        {
            // Arrange
            var model = new RegisterDTO { UserName = "user", Email = "email@test.com", Password = "password" };
            _authServiceMock.Setup(s => s.Register("user", "password", "email@test.com", "User"))
                            .Returns("Role 'User' does not exist!");

            // Act
            var result = _controller.Register(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Role 'User' does not exist!", badRequestResult.Value);
        }

        [Fact]
        public void Register_ShouldReturnOk_WhenRegistrationSucceeds()
        {
            // Arrange
            var model = new RegisterDTO { UserName = "user", Email = "email@test.com", Password = "password" };
            _authServiceMock.Setup(s => s.Register("user", "password", "email@test.com", "User"))
                            .Returns("Registration successful!");

            // Act
            var result = _controller.Register(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Registration successful!", okResult.Value);
        }

        [Fact]
        public void Login_ShouldReturnUnauthorized_WhenLoginFails()
        {
            // Arrange
            var model = new LoginDTO { UserName = "user", Password = "wrong-password" };
            _authServiceMock.Setup(s => s.Login("user", "wrong-password"))
                            .Returns("Invalid credentials!");

            // Act
            var result = _controller.Login(model);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid credentials!", unauthorizedResult.Value);
        }

        [Fact]
        public void Login_ShouldReturnOk_WhenLoginSucceeds()
        {
            // Arrange
            var model = new LoginDTO { UserName = "user", Password = "password" };
            _authServiceMock.Setup(s => s.Login("user", "password"))
                            .Returns("Login successful!");

            // Act
            var result = _controller.Login(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Login successful!", okResult.Value);
        }
    }
}
