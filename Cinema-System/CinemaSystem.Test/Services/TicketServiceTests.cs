using AutoMapper;
using Cinema_System.DTOs;
using Cinema_System.Mapping;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Ardalis.Specification;
using Moq;

namespace CinemaSystem.Test.Services;

public class TicketServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGenericRepository<Ticket>> _mockTicketRepo;
    private readonly IMapper _mapper;
    private readonly TicketService _service;

    public TicketServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTicketRepo = new Mock<IGenericRepository<Ticket>>();
        _mockUnitOfWork.Setup(u => u.Tickets).Returns(_mockTicketRepo.Object);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = config.CreateMapper();

        _service = new TicketService(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public void GetTicketsByUser_ShouldReturnMappedTickets()
    {
        // Arrange
        var userId = 10;
        var tickets = new List<Ticket>
        {
            new Ticket { TicketId = 1, UserId = userId },
            new Ticket { TicketId = 2, UserId = userId }
        };
        _mockTicketRepo.Setup(r => r.GetListBySpec(It.IsAny<ISpecification<Ticket>>())).Returns(tickets);

        // Act
        var result = _service.GetTicketsByUser(userId).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(userId, result[0].UserId);
        Assert.Equal(userId, result[1].UserId);
    }

    [Fact]
    public void GetTicketById_ShouldReturnNull_WhenTicketDoesNotExist()
    {
        // Arrange
        _mockTicketRepo.Setup(r => r.GetById(1)).Returns((Ticket?)null);

        // Act
        var result = _service.GetTicketById(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetTicketById_ShouldReturnMappedTicket_WhenTicketExists()
    {
        // Arrange
        var ticket = new Ticket { TicketId = 1, UserId = 5 };
        _mockTicketRepo.Setup(r => r.GetById(1)).Returns(ticket);

        // Act
        var result = _service.GetTicketById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.UserId);
    }

    [Fact]
    public void BookTicket_ShouldInsertAndSave()
    {
        // Arrange
        var dto = new TicketDTO { TicketId = 1, UserId = 5 };

        // Act
        _service.BookTicket(dto);

        // Assert
        _mockTicketRepo.Verify(r => r.Insert(It.Is<Ticket>(t => t.UserId == 5)), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }
}
