using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Moq;
using Xunit;
using Ardalis.Specification;

namespace CinemaSystem.Test.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IGenericRepository<User>> _userRepoMock;
        private readonly Mock<IGenericRepository<Ticket>> _ticketRepoMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _userRepoMock = new Mock<IGenericRepository<User>>();
            _ticketRepoMock = new Mock<IGenericRepository<Ticket>>();

            _uowMock.Setup(u => u.Users).Returns(_userRepoMock.Object);
            _uowMock.Setup(u => u.Tickets).Returns(_ticketRepoMock.Object);

            _service = new UserService(_uowMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var expectedUser = new User { Id = 1, UserName = "TestUser" };
            _userRepoMock.Setup(r => r.GetById(1)).Returns(expectedUser);

            // Act
            var result = await _service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestUser", result.UserName);
            _userRepoMock.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetById(1)).Returns((User?)null);

            // Act
            var result = await _service.GetUserByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserPurchasesAsync_ShouldReturnTicketsForUser()
        {
            // Arrange
            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, UserId = 1, SeatRow = 5 },
                new Ticket { TicketId = 2, UserId = 1, SeatRow = 6 }
            };

            _ticketRepoMock.Setup(r => r.GetListBySpec(It.IsAny<ISpecification<Ticket>>()))
                           .Returns(tickets);

            // Act
            var result = await _service.GetUserPurchasesAsync(1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(5, result[0].SeatRow);
            _ticketRepoMock.Verify(r => r.GetListBySpec(It.IsAny<ISpecification<Ticket>>()), Times.Once);
        }
    }
}
