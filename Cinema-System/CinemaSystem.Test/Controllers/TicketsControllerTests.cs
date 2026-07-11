using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.Controllers;

namespace CinemaSystem.Test.Controllers;

public class TicketsControllerTests
{
    private readonly Mock<ITicketService> _mockTicketService;
    private readonly TicketController _controller;

    public TicketsControllerTests()
    {
        _mockTicketService = new Mock<ITicketService>();
        _controller = new TicketController(_mockTicketService.Object);
    }

    [Fact]
    public void GetTicketsByUser_ShouldReturnOkWithTickets()
    {
        // Arrange
        var tickets = new List<TicketDTO> { new TicketDTO { TicketId = 1, UserId = 12 } };
        _mockTicketService.Setup(s => s.GetTicketsByUser(12)).Returns(tickets);

        // Act
        var result = _controller.GetTicketsByUser(12);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTickets = Assert.IsAssignableFrom<IEnumerable<TicketDTO>>(okResult.Value);
        Assert.Single(returnedTickets);
    }

    [Fact]
    public void BookTicket_ShouldBookTicketAndReturnOk()
    {
        // Arrange
        var dto = new TicketDTO { TicketId = 1, UserId = 12 };

        // Act
        var result = _controller.BookTicket(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        _mockTicketService.Verify(s => s.BookTicket(dto), Times.Once);
    }
}
