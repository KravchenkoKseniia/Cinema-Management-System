using Cinema_System.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using Xunit;

namespace CinemaSystem.Test.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<RoleManager<Role>> _roleManagerMock;
        private readonly AuthenticationService _service;

        public AuthenticationServiceTests()
        {
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                contextAccessorMock.Object,
                claimsFactoryMock.Object,
                null!, null!, null!, null!);

            var roleStoreMock = new Mock<IRoleStore<Role>>();
            _roleManagerMock = new Mock<RoleManager<Role>>(
                roleStoreMock.Object, null!, null!, null!, null!);

            _service = new AuthenticationService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _roleManagerMock.Object);
        }

        [Fact]
        public void Register_ShouldReturnError_WhenRoleDoesNotExist()
        {
            // Arrange
            _roleManagerMock.Setup(m => m.FindByNameAsync("NonExistentRole"))
                .ReturnsAsync((Role?)null);

            // Act
            var result = _service.Register("username", "password", "email@example.com", "NonExistentRole");

            // Assert
            Assert.Equal("Role 'NonExistentRole' does not exist!", result);
        }

        [Fact]
        public void Register_ShouldReturnSuccess_WhenUserCreationSucceeds()
        {
            // Arrange
            var role = new Role { Id = 1, Name = "User" };
            _roleManagerMock.Setup(m => m.FindByNameAsync("User"))
                .ReturnsAsync(role);

            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _service.Register("username", "password", "email@example.com", "User");

            // Assert
            Assert.Equal("Registration successful!", result);
        }

        [Fact]
        public void Register_ShouldReturnErrors_WhenUserCreationFails()
        {
            // Arrange
            var role = new Role { Id = 1, Name = "User" };
            _roleManagerMock.Setup(m => m.FindByNameAsync("User"))
                .ReturnsAsync(role);

            var errors = new[] { new IdentityError { Description = "Password too short" } };
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors));

            // Act
            var result = _service.Register("username", "p", "email@example.com", "User");

            // Assert
            Assert.Equal("Password too short", result);
        }

        [Fact]
        public void Login_ShouldReturnError_WhenUserNotFound()
        {
            // Arrange
            _userManagerMock.Setup(m => m.FindByNameAsync("NonExistentUser"))
                .ReturnsAsync((User?)null);

            // Act
            var result = _service.Login("NonExistentUser", "password");

            // Assert
            Assert.Equal("User not found!", result);
        }

        [Fact]
        public void Login_ShouldReturnSuccess_WhenPasswordSignInSucceeds()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "username" };
            _userManagerMock.Setup(m => m.FindByNameAsync("username"))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(m => m.PasswordSignInAsync(user, "password", false, false))
                .ReturnsAsync(SignInResult.Success);

            // Act
            var result = _service.Login("username", "password");

            // Assert
            Assert.Equal("Login successful!", result);
        }

        [Fact]
        public void Login_ShouldReturnError_WhenPasswordSignInFails()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "username" };
            _userManagerMock.Setup(m => m.FindByNameAsync("username"))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(m => m.PasswordSignInAsync(user, "wrong-password", false, false))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = _service.Login("username", "wrong-password");

            // Assert
            Assert.Equal("Invalid credentials!", result);
        }

        [Fact]
        public void IsAdmin_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            _userManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync((User?)null);

            // Act
            var result = _service.IsAdmin(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAdmin_ShouldReturnTrue_WhenUserHasAdminRole()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "admin" };
            _userManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(m => m.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Admin" });

            // Act
            var result = _service.IsAdmin(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAdmin_ShouldReturnFalse_WhenUserDoesNotHaveAdminRole()
        {
            // Arrange
            var user = new User { Id = 2, UserName = "regular" };
            _userManagerMock.Setup(m => m.FindByIdAsync("2"))
                .ReturnsAsync(user);

            _userManagerMock.Setup(m => m.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            // Act
            var result = _service.IsAdmin(2);

            // Assert
            Assert.False(result);
        }
    }
}
