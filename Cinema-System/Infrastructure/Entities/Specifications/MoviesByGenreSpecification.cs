using Ardalis.Specification;

namespace Infrastructure.Entities.Specifications;

public sealed class MoviesByGenreSpecification : Specification<Movie>
{
    public MoviesByGenreSpecification(int genreId)
    {
        Query.Where(m => m.Genres.Any(g => g.GenreId == genreId)).Include(m => m.Genres);
    }
}