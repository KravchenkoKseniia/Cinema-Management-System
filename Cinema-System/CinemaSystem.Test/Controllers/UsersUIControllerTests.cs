using Cinema_System.Services.Interfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.ControllersUI;
using Xunit;

namespace CinemaSystem.Test.Controllers
{
    public class UsersUIControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UsersUIController _controller;

        public UsersUIControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UsersUIController(_userServiceMock.Object);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnViewWithUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "TestUser" };
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<User>(viewResult.Model);
            Assert.Equal("TestUser", model.UserName);
        }

        [Fact]
        public async Task Purchases_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.Purchases(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Purchases_ShouldReturnViewWithPurchases_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "TestUser" };
            var tickets = new List<Ticket> { new Ticket { TicketId = 1, SeatRow = 5 } };

            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);
            _userServiceMock.Setup(s => s.GetUserPurchasesAsync(1)).ReturnsAsync(tickets);

            // Act
            var result = await _controller.Purchases(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Ticket>>(viewResult.Model);
            Assert.Single(model);
        }
    }
}
