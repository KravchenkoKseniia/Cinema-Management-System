namespace Cinema_System.Constants;

public class TmdbEndpoints
{
    public const string BaseApiUrl = "https://api.themoviedb.org/3/";
    
    public static string BasePosterUrl => $"https://image.tmdb.org/t/p/w500";
    public static string BaseTrailerUrl => "https://www.youtube.com/watch?v=";
    
    public static string SearchByTitleEndpoint(string title) => $"{BaseApiUrl}search/movie?query={title}";
    public static string DetailsByIdEndpoint(int movieId) => $"{BaseApiUrl}movie/{movieId}";
    public static string NowPlayingEndpoint => $"{BaseApiUrl}movie/now_playing";
    public static string UpcomingEndpoint => $"{BaseApiUrl}movie/upcoming";
    
    
    public static string TrailerKeyByIdEndpoint(int movieId) => $"{BaseApiUrl}movie/{movieId}/videos";
    
    public static string PosterByPathEndpoint(string posterPath) => $"{BasePosterUrl}{posterPath}";
    public static string TrailerByKeyEndpoint(string key) => $"{BaseTrailerUrl}{key}";
}