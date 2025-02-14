using Cinema_System.Constants;
using Cinema_System.DTOs;
using Infrastructure.Entities.Specifications;
using Infrastructure.Interfaces;

namespace Cinema_System.Services
{
    public class TmdbService
    {
        private readonly ITmdbRepository _tmdbRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TmdbService(ITmdbRepository tmdbRepository, IUnitOfWork unitOfWork)
        {
            _tmdbRepository = tmdbRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MovieSearchResult?> SearchMoviesByTitleAsync(string title)
        {
            return await _tmdbRepository.FetchFromTmdbAsync<MovieSearchResult>(
                TmdbEndpoints.SearchByTitleEndpoint(title)
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

        public async Task<MovieDTO?> FetchMovieFromTmdbByIdAsync(int movieId)
        {
            var movie = await _tmdbRepository.FetchFromTmdbAsync<MovieSearchItem>(
                TmdbEndpoints.DetailsByIdEndpoint(movieId)
            );

            if (movie == null)
                return null;

            var allGenres = _unitOfWork.Genres.GetListBySpec(new AllGenresSpecification()).ToList();
            
            var trailerResponse = await _tmdbRepository.FetchFromTmdbAsync<MovieTrailerResult>(
                TmdbEndpoints.TrailerKeyByIdEndpoint(movieId)
            );

            string trailerKey = trailerResponse?.Results?
                .FirstOrDefault(v => v.Type == "Trailer" && v.Site == "YouTube")?.Key ?? "";

            var movieDTO = TmdbMovieProcessor.ConvertToMovieDto(movie, allGenres, trailerKey);

            return movieDTO;
        }
    }
}
