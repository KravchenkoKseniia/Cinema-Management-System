using System.Collections.Generic;
using System.Linq;
using Ardalis.Specification.EntityFrameworkCore;
using Infrastructure.Entities;
using Infrastructure.Entities.Specifications;
using Xunit;

namespace CinemaSystem.Test.Specifications
{
    public class AvailableSeatsSpecificationTests
    {
        [Fact]
        public void Specification_ShouldIncludeTicketsWithNoPaymentId()
        {
            // Arrange
            var sessionId = 10;
            var spec = new AvailableSeatsSpecification(sessionId);

            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, SessionId = sessionId, PaymentId = null },
                new Ticket { TicketId = 2, SessionId = sessionId, PaymentId = null }
            };

            // Act
            var evaluator = new SpecificationEvaluator();
            var result = evaluator.GetQuery(tickets.AsQueryable(), spec).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.TicketId == 1);
            Assert.Contains(result, t => t.TicketId == 2);
        }

        [Fact]
        public void Specification_ShouldFilterOutTicketsWithExistingPayment()
        {
            // Arrange
            var sessionId = 10;
            var spec = new AvailableSeatsSpecification(sessionId);

            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, SessionId = sessionId, PaymentId = 99 }, // вже оплачений
                new Ticket { TicketId = 2, SessionId = sessionId, PaymentId = null } // доступний
            };

            // Act
            var evaluator = new SpecificationEvaluator();
            var result = evaluator.GetQuery(tickets.AsQueryable(), spec).ToList();

            // Assert
            var filteredTicket = Assert.Single(result);
            Assert.Null(filteredTicket.PaymentId);
            Assert.Equal(2, filteredTicket.TicketId);
        }

        [Fact]
        public void Specification_ShouldOnlyLookForSpecifiedSession()
        {
            // Arrange
            var targetSessionId = 5;
            var otherSessionId = 6;
            var spec = new AvailableSeatsSpecification(targetSessionId);

            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = 1, SessionId = targetSessionId, PaymentId = null },
                new Ticket { TicketId = 2, SessionId = otherSessionId, PaymentId = null } // рравильний статус, але не той сеанс
            };

            // Act
            var evaluator = new SpecificationEvaluator();
            var result = evaluator.GetQuery(tickets.AsQueryable(), spec).ToList();

            // Assert
            var filteredTicket = Assert.Single(result);
            Assert.Equal(targetSessionId, filteredTicket.SessionId);
        }
    }
}