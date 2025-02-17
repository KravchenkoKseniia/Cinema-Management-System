using Cinema_System.Constants;
using Cinema_System.DTOs;
using Infrastructure.Entities;

namespace Cinema_System.Services
{
    public static class TmdbMovieProcessor
    {
        public static MovieDTO ConvertToMovieDto(MovieSearchItemDTO movie, List<Genre> allGenres, string trailerKey)
        {
            var matchedGenres = allGenres
                .Where(g => movie.GenreIds.Contains(g.GenreId))
                .ToList();

            return new MovieDTO
            {
                MovieId = movie.Id,
                Title = movie.Title,
                Description = movie.Overview,
                TrailerURL = TmdbEndpoints.TrailerByKeyEndpoint(trailerKey),
                ReleaseDate = movie.ReleaseDate ?? DateTime.MinValue,
                Rating = movie.VoteAverage,
                Duration = TimeSpan.FromMinutes(movie.Runtime),
                PosterURL = TmdbEndpoints.PosterByPathEndpoint(movie.PosterPath),
                GenreIds = matchedGenres.Select(g => g.GenreId).ToList(),
                GenreNames = matchedGenres.Select(g => g.GenreName).ToList()
            };
        }
    }
}