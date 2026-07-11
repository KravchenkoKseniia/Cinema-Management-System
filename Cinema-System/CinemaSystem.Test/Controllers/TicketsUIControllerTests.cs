using System.Security.Claims;
using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.ControllersUI;

namespace CinemaSystem.Test.Controllers;

public class TicketsUIControllerTests
{
    private readonly Mock<ITicketService> _mockTicketService;
    private readonly TicketsUIController _controller;

    public TicketsUIControllerTests()
    {
        _mockTicketService = new Mock<ITicketService>();
        _controller = new TicketsUIController(_mockTicketService.Object);
    }

    [Fact]
    public void Index_ShouldReturnBadRequest_WhenUserIdClaimIsMissingOrInvalid()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity()); // No claims
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = _controller.Index();

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public void Index_ShouldReturnViewWithTickets_WhenUserIdClaimIsValid()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "42")
        }));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var tickets = new List<TicketDTO> { new TicketDTO { TicketId = 1, UserId = 42 } };
        _mockTicketService.Setup(s => s.GetTicketsByUser(42)).Returns(tickets);

        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(tickets, viewResult.Model);
    }

    [Fact]
    public void Book_Get_ShouldReturnView()
    {
        // Act
        var result = _controller.Book();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Book_Post_ShouldRedirectToIndex_WhenModelStateIsValid()
    {
        // Arrange
        var dto = new TicketDTO { TicketId = 1 };

        // Act
        var result = _controller.Book(dto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        _mockTicketService.Verify(s => s.BookTicket(dto), Times.Once);
    }

    [Fact]
    public void Book_Post_ShouldReturnViewWithModel_WhenModelStateIsInvalid()
    {
        // Arrange
        var dto = new TicketDTO { TicketId = 1 };
        _controller.ModelState.AddModelError("SeatRow", "Required");

        // Act
        var result = _controller.Book(dto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(dto, viewResult.Model);
        _mockTicketService.Verify(s => s.BookTicket(dto), Times.Never);
    }
}
