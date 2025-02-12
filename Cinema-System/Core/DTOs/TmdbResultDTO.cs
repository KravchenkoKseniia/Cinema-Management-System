using System.Text.Json.Serialization;


public class MovieSearchResult
{
    public List<MovieSearchItem> Results { get; set; } = new List<MovieSearchItem>();
}

public class MovieSearchItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
    
    [JsonPropertyName("overview")]
    public string Overview { get; set; } = null!;
    
    [JsonPropertyName("release_date")]
    public DateTime? ReleaseDate { get; set; } = null!;
    
    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }
    
    [JsonPropertyName("runtime")]
    public TimeSpan Runtime { get; set; }
    
    [JsonPropertyName("poster_path")]
    public string PosterPath { get; set; } = null!;
    
    [JsonPropertyName("genres")]
    public List<TmdbGenre> Genres { get; set; } = new List<TmdbGenre>();

    
    [JsonPropertyName("genre_ids")]
    public List<int> GenreIdsFromIds { get; set; } = new List<int>();
    
    public List<int> GenreIds => Genres.Any() 
        ? Genres.Select(g => g.Id).ToList()
        : GenreIdsFromIds;
}

public class TmdbGenre
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}