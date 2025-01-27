using Ardalis.Specification;

namespace CMS.Infrastructure.Entities.Specifications;

public sealed class SessionsByMovieAndDataSpecification : Specification<Session>
{
    public SessionsByMovieAndDataSpecification(int movieId, DateTime date)
    {
        Query.Where(session => session.MovieId == movieId && session.Date.Date == date.Date)
            .Include(session => session.Hall)
            .Include(session => session.Movie);
    }
}