using Cinema_System.Services.Interfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.Controllers;
using Xunit;

namespace CinemaSystem.Test.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UsersController(_userServiceMock.Object);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetUserById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnOkWithUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "TestUser" };
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal("TestUser", returnedUser.UserName);
        }

        [Fact]
        public async Task GetUserPurchases_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetUserPurchases(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetUserPurchases_ShouldReturnOkWithPurchases_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "TestUser" };
            var tickets = new List<Ticket> { new Ticket { TicketId = 1, SeatRow = 5 } };
            
            _userServiceMock.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(user);
            _userServiceMock.Setup(s => s.GetUserPurchasesAsync(1)).ReturnsAsync(tickets);

            // Act
            var result = await _controller.GetUserPurchases(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Single(returnedTickets);
            Assert.Equal(5, returnedTickets[0].SeatRow);
        }
    }
}
