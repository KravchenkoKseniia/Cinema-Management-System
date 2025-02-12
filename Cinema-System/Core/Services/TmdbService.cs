using Cinema_System.Constants;
using Infrastructure.Interfaces;

namespace Cinema_System.Services
{
    public class TmdbService
    {
        private readonly ITmdbRepository _tmdbRepository;

        public TmdbService(ITmdbRepository tmdbRepository)
        {
            _tmdbRepository = tmdbRepository;
        }
        
        public async Task<MovieSearchResult?> SearchMoviesByTitleAsync(string title)
        {
            return await _tmdbRepository.FetchFromTmdbAsync<MovieSearchResult>(
                TmdbEndpoints.SearchByTitleEndpoint(title)
            );
        }
        
        public async Task<MovieSearchItem?> FetchMovieFromTmdbByIdAsync(int movieId)
        {
            return await _tmdbRepository.FetchFromTmdbAsync<MovieSearchItem>(
                TmdbEndpoints.DetailsByIdEndpoint(movieId)
            );
        }

        public async Task<MovieSearchResult?> GetNowPlayingMoviesAsync()
        {
            return await _tmdbRepository.FetchFromTmdbAsync<MovieSearchResult>(
                TmdbEndpoints.NowPlayingEndpoint
            );
        }

        public async Task<MovieSearchResult?> GetUpcomingMoviesAsync()
        {
            return await _tmdbRepository.FetchFromTmdbAsync<MovieSearchResult>(
                TmdbEndpoints.UpcomingEndpoint
            );
        }
        
        // TODO: add logic to fetch trailer key from TMDB
        // TODO: add logic to convert MovieSearchItem to MovieDTO
    }
}
