using Ardalis.Specification;

namespace Infrastructure.Entities.Specifications;

public sealed class TicketsByUserSpecification : Specification<Ticket>
{
    public TicketsByUserSpecification(int userId)
    {
        Query.Where(ticket => ticket.UserId == userId)
            .Include(ticket => ticket.Session)
            .ThenInclude(session => session.Movie);
    }
}