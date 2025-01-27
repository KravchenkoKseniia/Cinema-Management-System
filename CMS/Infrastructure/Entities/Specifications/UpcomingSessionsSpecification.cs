using Ardalis.Specification;
namespace CMS.Infrastructure.Entities.Specifications;

public sealed class UpcomingSessionsSpecification : Specification<Session>
{
    public UpcomingSessionsSpecification()
    {
        Query.Where(session => session.StartTime > DateTime.Now.TimeOfDay)
            .Include(session => session.Movie)
            .Include(session => session.Hall);
    }
}