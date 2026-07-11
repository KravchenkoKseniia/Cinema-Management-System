using System;
using System.Collections.Generic;
using Cinema_System.Constants;
using Cinema_System.DTOs;
using Cinema_System.Services;
using Infrastructure.Entities;
using Xunit;

namespace CinemaSystem.Test
{
    public class TmdbMovieProcessorTests
    {
        [Fact]
        public void ConvertToMovieDto_ShouldMapMatchedGenres()
        {
            // Arrange
            var movie = new MovieSearchItemDTO
            {
                Id = 1,
                Title = "Inception",
                Overview = "A thief who steals corporate secrets through the use of dream-sharing technology.",
                ReleaseDateString = "2010-07-16",
                VoteAverage = 8.8m,
                Runtime = 148,
                PosterPath = "/poster.jpg",
                Genres = new List<TmdbGenre>
                {
                    new TmdbGenre { Id = 28, Name = "Action" },
                    new TmdbGenre { Id = 878, Name = "Sci-Fi" }
                }
            };

            var allGenres = new List<Genre>
            {
                new Genre { GenreId = 28, GenreName = "Action" },
                new Genre { GenreId = 878, GenreName = "Sci-Fi" },
                new Genre { GenreId = 35, GenreName = "Comedy" }
            };

            // Act
            var result = TmdbMovieProcessor.ConvertToMovieDto(movie, allGenres, "trailer_key");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.GenreIds.Count);
            Assert.Contains(28, result.GenreIds);
            Assert.Contains(878, result.GenreIds);
            Assert.Equal(2, result.GenreNames.Count);
            Assert.Contains("Action", result.GenreNames);
            Assert.Contains("Sci-Fi", result.GenreNames);
            Assert.DoesNotContain("Comedy", result.GenreNames);
        }

        [Fact]
        public void ConvertToMovieDto_ShouldIgnoreUnmatchedGenres()
        {
            // Arrange
            var movie = new MovieSearchItemDTO
            {
                Id = 2,
                Title = "Interstellar",
                Overview = "A team of explorers travel through a wormhole in space.",
                ReleaseDateString = "2014-11-07",
                VoteAverage = 8.6m,
                Runtime = 169,
                PosterPath = "/interstellar.jpg",
                Genres = new List<TmdbGenre>
                {
                    new TmdbGenre { Id = 878, Name = "Sci-Fi" },
                    new TmdbGenre { Id = 999, Name = "UnknownGenre" }
                }
            };

            var allGenres = new List<Genre>
            {
                new Genre { GenreId = 878, GenreName = "Sci-Fi" }
            };

            // Act
            var result = TmdbMovieProcessor.ConvertToMovieDto(movie, allGenres, "trailer_key_2");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.GenreIds);
            Assert.Equal(878, result.GenreIds[0]);
            Assert.Single(result.GenreNames);
            Assert.Equal("Sci-Fi", result.GenreNames[0]);
        }

        [Fact]
        public void ConvertToMovieDto_ShouldHandleEmptyGenres()
        {
            // Arrange
            var movie = new MovieSearchItemDTO
            {
                Id = 3,
                Title = "Untitled Movie",
                Overview = "No overview.",
                ReleaseDateString = "2025-01-01",
                VoteAverage = 5.0m,
                Runtime = 90,
                PosterPath = "/empty.jpg",
                Genres = new List<TmdbGenre>()
            };

            var allGenres = new List<Genre>
            {
                new Genre { GenreId = 28, GenreName = "Action" }
            };

            // Act
            var result = TmdbMovieProcessor.ConvertToMovieDto(movie, allGenres, "key");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.GenreIds);
            Assert.Empty(result.GenreNames);
        }

        [Fact]
        public void ConvertToMovieDto_ShouldConvertValidReleaseDate()
        {
            // Arrange
            var movie = new MovieSearchItemDTO
            {
                Id = 4,
                Title = "Test Movie",
                ReleaseDateString = "2024-05-15",
                Genres = new List<TmdbGenre>()
            };

            var allGenres = new List<Genre>();

            // Act
            var result = TmdbMovieProcessor.ConvertToMovieDto(movie, allGenres, "key");

            // Assert
            Assert.Equal(new DateTime(2024, 05, 15), result.ReleaseDate);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid-date")]
        [InlineData("2024-13-45")]
        public void ConvertToMovieDto_ShouldFallbackToMinValueForInvalidOrEmptyReleaseDate(string invalidDate)
        {
            // Arrange
            var movie = new MovieSearchItemDTO
            {
                Id = 5,
                Title = "Test Movie Invalid Date",
                ReleaseDateString = invalidDate,
                Genres = new List<TmdbGenre>()
            };

            var allGenres = new List<Genre>();

            // Act
            var result = TmdbMovieProcessor.ConvertToMovieDto(movie, allGenres, "key");

            // Assert
            Assert.Equal(DateTime.MinValue, result.ReleaseDate);
        }

        [Fact]
        public void ConvertToMovieDto_ShouldFallbackToMinValueForNullReleaseDate()
        {
            // Arrange
            var movie = new MovieSearchItemDTO
            {
                Id = 6,
                Title = "Test Movie Null Date",
                ReleaseDateString = null!,
                Genres = new List<TmdbGenre>()
            };

            var allGenres = new List<Genre>();

            // Act
            var result = TmdbMovieProcessor.ConvertToMovieDto(movie, allGenres, "key");

            // Assert
            Assert.Equal(DateTime.MinValue, result.ReleaseDate);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(120, 120)]
        [InlineData(185, 185)]
        public void ConvertToMovieDto_ShouldConvertDurationFromRuntimeMinutes(int runtimeMinutes, int expectedMinutes)
        {
            // Arrange
            var movie = new MovieSearchItemDTO
            {
                Id = 7,
                Title = "Test Runtime",
                Runtime = runtimeMinutes,
                Genres = new List<TmdbGenre>()
            };

            var allGenres = new List<Genre>();

            // Act
            var result = TmdbMovieProcessor.ConvertToMovieDto(movie, allGenres, "key");

            // Assert
            Assert.Equal(TimeSpan.FromMinutes(expectedMinutes), result.Duration);
        }

        [Fact]
        public void ConvertToMovieDto_ShouldGenerateCorrectUrls()
        {
            // Arrange
            var posterPath = "/my_poster_image.png";
            var trailerKey = "dQw4w9WgXcQ";

            var movie = new MovieSearchItemDTO
            {
                Id = 8,
                Title = "URL Test Movie",
                PosterPath = posterPath,
                Genres = new List<TmdbGenre>()
            };

            var allGenres = new List<Genre>();

            // Act
            var result = TmdbMovieProcessor.ConvertToMovieDto(movie, allGenres, trailerKey);

            // Assert
            var expectedPosterUrl = TmdbEndpoints.PosterByPathEndpoint(posterPath);
            var expectedTrailerUrl = TmdbEndpoints.TrailerByKeyEndpoint(trailerKey);

            Assert.Equal(expectedPosterUrl, result.PosterURL);
            Assert.Equal(expectedTrailerUrl, result.TrailerURL);
            Assert.Equal($"https://image.tmdb.org/t/p/w500{posterPath}", result.PosterURL);
            Assert.Equal($"https://www.youtube.com/watch?v={trailerKey}", result.TrailerURL);
        }
    }
}
