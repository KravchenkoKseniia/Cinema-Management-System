namespace CMS.Models;

public class Genre
{
    public int GenreId { get; set; }
    public string GenreName { get; set; } = null!;

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
