using Ardalis.Specification;

namespace CMS.Infrastructure.Entities.Specifications;

public sealed class MoviesByGenreSpecification : Specification<Movie>
{
    public MoviesByGenreSpecification(int genreId)
    {
        Query.Where(m => m.GenreId == genreId).Include(m => m.Genre);
    }
}