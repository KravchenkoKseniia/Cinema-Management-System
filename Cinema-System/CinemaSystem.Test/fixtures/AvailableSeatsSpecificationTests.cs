using System.Collections.Generic;
using System.Linq;
using Ardalis.Specification.EntityFrameworkCore;
using Infrastructure.Entities;
using Infrastructure.Entities.Specifications;
using FluentAssertions;

namespace CinemaSystem.Test.Specifications;

public class AvailableSeatsSpecificationTests
{
    [Fact]
    public void GetQuery_SessionTicketsWithNoPaymentId_IncludesAllTickets()
    {
        // Arrange
        var sessionId = 10;
        var spec = new AvailableSeatsSpecification(sessionId);

        var tickets = new List<Ticket>
        {
            new Ticket { TicketId = 1, SessionId = sessionId, PaymentId = 0 },
            new Ticket { TicketId = 2, SessionId = sessionId, PaymentId = 0 }
        };

        // Act
        var evaluator = new SpecificationEvaluator();
        var result = evaluator.GetQuery(tickets.AsQueryable(), spec).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.TicketId == 1);
        result.Should().Contain(t => t.TicketId == 2);
    }

    [Fact]
    public void GetQuery_TicketsWithExistingPayment_FiltersOutPaidTickets()
    {
        // Arrange
        var sessionId = 10;
        var spec = new AvailableSeatsSpecification(sessionId);

        var tickets = new List<Ticket>
        {
            new Ticket { TicketId = 1, SessionId = sessionId, PaymentId = 99 }, // Already paid
            new Ticket { TicketId = 2, SessionId = sessionId, PaymentId = 0 } // Available (unpaid = 0)
        };

        // Act
        var evaluator = new SpecificationEvaluator();
        var result = evaluator.GetQuery(tickets.AsQueryable(), spec).ToList();

        // Assert
        var filteredTicket = result.Should().ContainSingle().Subject;
        filteredTicket.PaymentId.Should().Be(0);
        filteredTicket.TicketId.Should().Be(2);
    }

    [Fact]
    public void GetQuery_TicketsFromDifferentSessions_FiltersToSpecifiedSession()
    {
        // Arrange
        var targetSessionId = 5;
        var otherSessionId = 6;
        var spec = new AvailableSeatsSpecification(targetSessionId);

        var tickets = new List<Ticket>
        {
            new Ticket { TicketId = 1, SessionId = targetSessionId, PaymentId = 0 },
            new Ticket { TicketId = 2, SessionId = otherSessionId, PaymentId = 0 } // Right status, wrong session
        };

        // Act
        var evaluator = new SpecificationEvaluator();
        var result = evaluator.GetQuery(tickets.AsQueryable(), spec).ToList();

        // Assert
        var filteredTicket = result.Should().ContainSingle().Subject;
        filteredTicket.SessionId.Should().Be(5);
    }
}
