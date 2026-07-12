using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Cinema_System.Services;
using Infrastructure.Entities;
using Moq;
using FluentAssertions;

namespace CinemaSystem.Test.Services;

public class AuthenticationServiceTests
{
    private readonly Mock<IUserStore<User>> _userStoreMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IRoleStore<Role>> _roleStoreMock;
    private readonly Mock<RoleManager<Role>> _roleManagerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IUserClaimsPrincipalFactory<User>> _claimsPrincipalFactoryMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly AuthenticationService _authService;

    public AuthenticationServiceTests()
    {
        _userStoreMock = new Mock<IUserStore<User>>(MockBehavior.Strict);
        
        // Mocking UserManager
        _userManagerMock = new Mock<UserManager<User>>(
            _userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _roleStoreMock = new Mock<IRoleStore<Role>>(MockBehavior.Strict);
        
        // Mocking RoleManager
        _roleManagerMock = new Mock<RoleManager<Role>>(
            _roleStoreMock.Object, null!, null!, null!, null!);

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>(MockBehavior.Strict);
        _claimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>(MockBehavior.Strict);

        // Mocking SignInManager
        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object,
            _httpContextAccessorMock.Object,
            _claimsPrincipalFactoryMock.Object,
            null!, null!, null!, null!);

        _authService = new AuthenticationService(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _roleManagerMock.Object);
    }

    [Fact]
    public void Register_RoleDoesNotExist_ReturnsRoleDoesNotExistMessage()
    {
        // Arrange
        // NOTE: AuthenticationService uses .Result which is a sync-over-async smell.
        _roleManagerMock
            .Setup(r => r.FindByNameAsync("Admin"))
            .ReturnsAsync((Role)null!);

        // Act
        var result = _authService.Register("username", "Password123!", "email@test.com", "Admin");

        // Assert
        result.Should().Be("Role 'Admin' does not exist!");
    }

    [Fact]
    public void Register_ValidUserAndRole_CreatesUserAndReturnsSuccessMessage()
    {
        // Arrange
        var role = new Role { Id = 2, Name = "User" };
        _roleManagerMock
            .Setup(r => r.FindByNameAsync("User"))
            .ReturnsAsync(role);

        _userManagerMock
            .Setup(u => u.CreateAsync(It.Is<User>(u => 
                u.UserName == "bob" && 
                u.Email == "bob@mail.com" && 
                u.RoleId == 2
            ), "Password123!"))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = _authService.Register("bob", "Password123!", "bob@mail.com", "User");

        // Assert
        result.Should().Be("Registration successful!");
        _userManagerMock.Verify(u => u.CreateAsync(It.IsAny<User>(), "Password123!"), Times.Once);
    }

    [Fact]
    public void Register_UserManagerCreateFails_ReturnsJoinedErrorDescriptions()
    {
        // Arrange
        var role = new Role { Id = 2, Name = "User" };
        _roleManagerMock
            .Setup(r => r.FindByNameAsync("User"))
            .ReturnsAsync(role);

        var errors = new[]
        {
            new IdentityError { Description = "Password too short" },
            new IdentityError { Description = "Username taken" }
        };

        _userManagerMock
            .Setup(u => u.CreateAsync(It.IsAny<User>(), "short"))
            .ReturnsAsync(IdentityResult.Failed(errors));

        // Act
        var result = _authService.Register("bob", "short", "bob@mail.com", "User");

        // Assert
        result.Should().Be("Password too short, Username taken");
    }

    [Fact]
    public void Login_UserDoesNotExist_ReturnsUserNotFoundMessage()
    {
        // SMELL: Username enumeration security issue (returns "User not found!" instead of a generic "Invalid credentials!").
        
        // Arrange
        _userManagerMock
            .Setup(u => u.FindByNameAsync("nonexistent"))
            .ReturnsAsync((User)null!);

        // Act
        var result = _authService.Login("nonexistent", "Password123!");

        // Assert
        result.Should().Be("User not found!");
    }

    [Fact]
    public void Login_ValidCredentials_ReturnsLoginSuccessfulMessage()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "bob" };
        _userManagerMock
            .Setup(u => u.FindByNameAsync("bob"))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(s => s.PasswordSignInAsync(user, "Password123!", false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        // Act
        var result = _authService.Login("bob", "Password123!");

        // Assert
        result.Should().Be("Login successful!");
    }

    [Fact]
    public void Login_InvalidCredentials_ReturnsInvalidCredentialsMessage()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "bob" };
        _userManagerMock
            .Setup(u => u.FindByNameAsync("bob"))
            .ReturnsAsync(user);

        _signInManagerMock
            .Setup(s => s.PasswordSignInAsync(user, "WrongPassword", false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        // Act
        var result = _authService.Login("bob", "WrongPassword");

        // Assert
        result.Should().Be("Invalid credentials!");
    }

    [Fact]
    public void IsAdmin_UserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _userManagerMock
            .Setup(u => u.FindByIdAsync("999"))
            .ReturnsAsync((User)null!);

        // Act
        var result = _authService.IsAdmin(999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsAdmin_UserIsNotAdmin_ReturnsFalse()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "bob" };
        _userManagerMock
            .Setup(u => u.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        // Act
        var result = _authService.IsAdmin(1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsAdmin_UserIsAdmin_ReturnsTrue()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "admin" };
        _userManagerMock
            .Setup(u => u.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(u => u.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Admin" });

        // Act
        var result = _authService.IsAdmin(1);

        // Assert
        result.Should().BeTrue();
    }
}
