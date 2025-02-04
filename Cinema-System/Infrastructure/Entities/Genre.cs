using Infrastructure.Interfaces;

namespace Infrastructure.Entities;

public class Genre : IBaseEntity
{
    public int GenreId { get; set; }
    public string GenreName { get; set; } = null!;

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}