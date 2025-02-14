using Ardalis.Specification;

namespace Infrastructure.Entities.Specifications;

public sealed class SessionsByMovieSpecification : Specification<Session>
{
    public SessionsByMovieSpecification(int movieId)
    {
        Query.Where(session => session.MovieId == movieId);
    }
}