namespace Cinema_System.DTOs;

public class MovieDTO
{
    public int MovieId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string TrailerURL { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public decimal Rating { get; set; }
    public TimeSpan Duration { get; set; }
    public string PosterURL { get; set; } = null!;
    public int GenreId { get; set; }
    public string GenreName { get; set; } = null!;
}