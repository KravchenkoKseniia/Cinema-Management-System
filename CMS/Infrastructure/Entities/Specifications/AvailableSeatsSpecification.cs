using Ardalis.Specification;
using CMS.Infrastructure.Entities;
namespace CMS.Infrastructure.Entities.Specifications;

public class AvailableSeatsSpecification : Specification<Ticket>
{
    public AvailableSeatsSpecification(int sessionId)
    {
        Query.Where(ticket => ticket.SessionId == sessionId && ticket.PaymentId == null)
            .Include(ticket => ticket.Session)
            .ThenInclude(session => session.Hall);
    }
}