using System.Text.Json.Serialization;

namespace Cinema_System.DTOs;

public class MovieSearchResultDTO
{
    public List<MovieSearchItemDTO> Results { get; set; } = new List<MovieSearchItemDTO>();
}

public class MovieSearchItemDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
    
    [JsonPropertyName("overview")]
    public string Overview { get; set; } = null!;
    
    [JsonPropertyName("release_date")]
    public string ReleaseDateString { get; set; } = null!;
    
    [JsonIgnore]
    public DateTime? ReleaseDate => DateTime.TryParse(ReleaseDateString, out var date) ? date : (DateTime?)null;
    
    [JsonPropertyName("vote_average")]
    public decimal VoteAverage { get; set; }
    
    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }
    
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


public class MovieTrailerResult
{
    [JsonPropertyName("results")]
    public List<MovieTrailerItem> Results { get; set; } = new();
}

public class MovieTrailerItem
{
    [JsonPropertyName("key")] public string Key { get; set; } = null!;

    [JsonPropertyName("site")] public string Site { get; set; } = null!;

    [JsonPropertyName("type")] public string Type { get; set; } = null!;
}