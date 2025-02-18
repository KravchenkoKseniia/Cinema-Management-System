using Cinema_System.DTOs;

namespace Cinema_System.Services.Interfaces
{
    public interface ITmdbService
    {
        Task<MovieSearchResultDTO?> SearchMoviesByTitleAsync(string title);
        Task<MovieSearchResultDTO?> GetNowPlayingMoviesAsync();
        Task<MovieSearchResultDTO?> GetUpcomingMoviesAsync();
        Task<MovieDTO?> FetchMovieFromTmdbByIdAsync(int movieId);
    }
}