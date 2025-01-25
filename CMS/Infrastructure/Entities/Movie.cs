namespace CMS.Infrastructure.Entities;

public class Movie
{
    public int MovieId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string TrailerURL { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public int GenreId { get; set; }
    public decimal Rating { get; set; }
    public TimeSpan Duration { get; set; }
    public string PosterURL { get; set; } = null!;

    public Genre Genre { get; set; } = null!;
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}
