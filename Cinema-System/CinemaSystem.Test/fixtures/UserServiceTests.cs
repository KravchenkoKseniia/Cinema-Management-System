using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Moq;
using FluentAssertions;

namespace CinemaSystem.Test.Services;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IGenericRepository<User>> _userRepoMock;
    private readonly Mock<IGenericRepository<Ticket>> _ticketRepoMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        _userRepoMock = new Mock<IGenericRepository<User>>(MockBehavior.Strict);
        _ticketRepoMock = new Mock<IGenericRepository<Ticket>>(MockBehavior.Strict);

        _uowMock.Setup(uow => uow.Users).Returns(_userRepoMock.Object);
        _uowMock.Setup(uow => uow.Tickets).Returns(_ticketRepoMock.Object);

        _userService = new UserService(_uowMock.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_UserExists_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 10, UserName = "bob" };
        _userRepoMock.Setup(r => r.GetById(10)).Returns(user);

        // Act
        var result = await _userService.GetUserByIdAsync(10);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(10);
        result.UserName.Should().Be("bob");
    }

    [Fact]
    public async Task GetUserByIdAsync_UserDoesNotExist_ReturnsNull()
    {
        // Arrange
        _userRepoMock.Setup(r => r.GetById(99)).Returns((User)null!);

        // Act
        var result = await _userService.GetUserByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetUserPurchasesAsync_UserHasPurchases_ReturnsPurchasesList()
    {
        // Arrange
        var userId = 5;
        var tickets = new List<Ticket>
        {
            new Ticket { TicketId = 1, UserId = userId, SessionId = 20, SeatRow = 2 },
            new Ticket { TicketId = 2, UserId = userId, SessionId = 30, SeatRow = 4 }
        };

        _ticketRepoMock
            .Setup(r => r.GetListBySpec(It.IsAny<TicketsByUserSpecification>()))
            .Returns(tickets);

        // Act
        var result = await _userService.GetUserPurchasesAsync(userId);

        // Assert
        result.Should().HaveCount(2);
        result[0].TicketId.Should().Be(1);
        result[1].TicketId.Should().Be(2);
        
        _ticketRepoMock.Verify(r => r.GetListBySpec(It.IsAny<TicketsByUserSpecification>()), Times.Once);
    }
}
