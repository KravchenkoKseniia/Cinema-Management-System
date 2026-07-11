using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.ControllersUI;
using Xunit;

namespace CinemaSystem.Test.Controllers
{
    public class AuthorizationUIControllerTests
    {
        private readonly Mock<IAuthenticationService> _authServiceMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly AuthorizationUIController _controller;

        public AuthorizationUIControllerTests()
        {
            _authServiceMock = new Mock<IAuthenticationService>();

            var userStoreMock = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(
                userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
            _signInManagerMock = new Mock<SignInManager<User>>(
                userManagerMock.Object,
                contextAccessorMock.Object,
                claimsFactoryMock.Object,
                null!, null!, null!, null!);

            _controller = new AuthorizationUIController(_authServiceMock.Object, _signInManagerMock.Object);
        }

        [Fact]
        public void Login_Get_ShouldReturnView()
        {
            // Act
            var result = _controller.Login();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void Login_Post_ShouldReturnViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("UserName", "Required");
            var model = new LoginDTO();

            // Act
            var result = _controller.Login(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public void Login_Post_ShouldRedirectToSessionsUIIndex_WhenAdminCredentialsMatch()
        {
            // Arrange
            var model = new LoginDTO { UserName = "admin", Password = "passwordPassword123@@@" };

            // Act
            var result = _controller.Login(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("SessionsUI", redirectResult.ControllerName);
        }

        [Fact]
        public void Login_Post_ShouldRedirectToHomeIndex_WhenLoginSucceeds()
        {
            // Arrange
            var model = new LoginDTO { UserName = "user", Password = "password" };
            _authServiceMock.Setup(s => s.Login("user", "password")).Returns("Login successful!");

            // Act
            var result = _controller.Login(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);
        }

        [Fact]
        public void Login_Post_ShouldReturnViewWithModelError_WhenLoginFails()
        {
            // Arrange
            var model = new LoginDTO { UserName = "user", Password = "wrong" };
            _authServiceMock.Setup(s => s.Login("user", "wrong")).Returns("Invalid credentials!");

            // Act
            var result = _controller.Login(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
            Assert.True(_controller.ModelState.ContainsKey(""));
        }

        [Fact]
        public void Register_Get_ShouldReturnView()
        {
            // Act
            var result = _controller.Register();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void Register_Post_ShouldReturnViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("UserName", "Required");
            var model = new RegisterDTO();

            // Act
            var result = _controller.Register(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public void Register_Post_ShouldRedirectToLogin_WhenRegistrationSucceeds()
        {
            // Arrange
            var model = new RegisterDTO { UserName = "user", Password = "password", Email = "email" };
            _authServiceMock.Setup(s => s.Register("user", "password", "email", "User"))
                            .Returns("Registration successful!");

            // Act
            var result = _controller.Register(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
        }

        [Fact]
        public void Register_Post_ShouldReturnViewWithModelError_WhenRegistrationFails()
        {
            // Arrange
            var model = new RegisterDTO { UserName = "user", Password = "password", Email = "email" };
            _authServiceMock.Setup(s => s.Register("user", "password", "email", "User"))
                            .Returns("Some error");

            // Act
            var result = _controller.Register(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
            Assert.True(_controller.ModelState.ContainsKey(""));
        }

        [Fact]
        public async Task Logout_ShouldSignOutAndRedirectToHomeIndex()
        {
            // Arrange
            _signInManagerMock.Setup(m => m.SignOutAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Logout();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);
            _signInManagerMock.Verify(m => m.SignOutAsync(), Times.Once);
        }
    }
}
