using System;
using AutoMapper;
using Cinema_System.DTOs;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Moq;
using FluentAssertions;

namespace CinemaSystem.Test.Services;

public class TicketServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IGenericRepository<Ticket>> _ticketRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TicketService _ticketService;

    public TicketServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        _ticketRepoMock = new Mock<IGenericRepository<Ticket>>(MockBehavior.Strict);
        _mapperMock = new Mock<IMapper>(MockBehavior.Strict);

        _uowMock.Setup(uow => uow.Tickets).Returns(_ticketRepoMock.Object);

        _ticketService = new TicketService(_uowMock.Object, _mapperMock.Object);
    }

    [Fact]
    public void BookTicket_ValidTicketDto_MapsAndPersistsTicket()
    {
        // Arrange
        var ticketDto = new TicketDTO
        {
            UserId = 42,
            SessionId = 10,
            SeatRow = 5,
            PaymentId = 99
        };

        var mappedTicket = new Ticket
        {
            UserId = 42,
            SessionId = 10,
            SeatRow = 5,
            PaymentId = 99
        };

        _mapperMock.Setup(m => m.Map<Ticket>(ticketDto)).Returns(mappedTicket);
        _ticketRepoMock.Setup(r => r.Insert(mappedTicket));
        _uowMock.Setup(uow => uow.Save()).Returns(1);

        // Act
        _ticketService.BookTicket(ticketDto);

        // Assert
        _ticketRepoMock.Verify(r => r.Insert(It.Is<Ticket>(t => 
            t.UserId == 42 && 
            t.SessionId == 10 && 
            t.SeatRow == 5 &&
            t.PaymentId == 99)), Times.Once);
        _uowMock.Verify(uow => uow.Save(), Times.Once);
    }

    [Fact]
    public void BookTicket_DbSaveFails_CallsSave()
    {
        // Arrange
        var ticketDto = new TicketDTO { UserId = 1, SessionId = 1 };
        var ticket = new Ticket { UserId = 1, SessionId = 1 };

        _mapperMock.Setup(m => m.Map<Ticket>(ticketDto)).Returns(ticket);
        _ticketRepoMock.Setup(r => r.Insert(ticket));
        _uowMock.Setup(uow => uow.Save()).Returns(0);

        // Act
        _ticketService.BookTicket(ticketDto);

        // Assert
        _uowMock.Verify(uow => uow.Save(), Times.Once);
    }
    
    [Fact]
    public void BookTicket_NullDto_ThrowsArgumentNullException()
    {
        // Arrange
        TicketDTO nullDto = null!;

        // Act
        Action act = () => _ticketService.BookTicket(nullDto);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
