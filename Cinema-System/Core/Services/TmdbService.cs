using Cinema_System.Constants;
using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using Infrastructure.Entities.Specifications;
using Infrastructure.Interfaces;

namespace Cinema_System.Services
{
    public class TmdbService : ITmdbService
    {
        private readonly ITmdbRepository _tmdbRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TmdbService(ITmdbRepository tmdbRepository, IUnitOfWork unitOfWork)
        {
            _tmdbRepository = tmdbRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MovieSearchResultDTO?> SearchMoviesByTitleAsync(string title)
        {
            return await _tmdbRepository.FetchFromTmdbAsync<MovieSearchResultDTO>(
                TmdbEndpoints.SearchByTitleEndpoint(title)
            );
        }

        public async Task<MovieSearchResultDTO?> GetNowPlayingMoviesAsync()
        {
            return await _tmdbRepository.FetchFromTmdbAsync<MovieSearchResultDTO>(
                TmdbEndpoints.NowPlayingEndpoint
            );
        }

        public async Task<MovieSearchResultDTO?> GetUpcomingMoviesAsync()
        {
            return await _tmdbRepository.FetchFromTmdbAsync<MovieSearchResultDTO>(
                TmdbEndpoints.UpcomingEndpoint
            );
        }

        public async Task<MovieDTO?> FetchMovieFromTmdbByIdAsync(int movieId)
        {
            var movie = await _tmdbRepository.FetchFromTmdbAsync<MovieSearchItemDTO>(
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
