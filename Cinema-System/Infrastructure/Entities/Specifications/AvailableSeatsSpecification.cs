using Ardalis.Specification;

namespace Infrastructure.Entities.Specifications;

public sealed class AvailableSeatsSpecification : Specification<Ticket>
{
    public AvailableSeatsSpecification(int sessionId)
    {
        Query.Where(ticket => ticket.SessionId == sessionId && ticket.PaymentId == null)
            .Include(ticket => ticket.Session)
            .ThenInclude(session => session.Hall);
    }
}