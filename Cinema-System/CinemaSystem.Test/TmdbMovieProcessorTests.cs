using System;
using System.Collections.Generic;
using Cinema_System.Constants;
using Cinema_System.DTOs;
using Cinema_System.Services;
using Infrastructure.Entities;
using FluentAssertions;

namespace CinemaSystem.Test;

public class TmdbMovieProcessorTests
{
    [Fact]
    public void ConvertToMovieDto_ValidMovieAndGenres_MapsMatchedGenres()
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
        result.Should().NotBeNull();
        result.GenreIds.Should().HaveCount(2)
            .And.Contain(28)
            .And.Contain(878);
        result.GenreNames.Should().HaveCount(2)
            .And.Contain("Action")
            .And.Contain("Sci-Fi")
            .And.NotContain("Comedy");
    }

    [Fact]
    public void ConvertToMovieDto_UnmatchedGenresPresent_IgnoresUnmatchedGenres()
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
        result.Should().NotBeNull();
        result.GenreIds.Should().ContainSingle().Which.Should().Be(878);
        result.GenreNames.Should().ContainSingle().Which.Should().Be("Sci-Fi");
    }

    [Fact]
    public void ConvertToMovieDto_EmptyGenres_ReturnsEmptyGenreLists()
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
        result.Should().NotBeNull();
        result.GenreIds.Should().BeEmpty();
        result.GenreNames.Should().BeEmpty();
    }

    [Fact]
    public void ConvertToMovieDto_ValidReleaseDateString_ConvertsReleaseDate()
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
        result.ReleaseDate.Should().Be(new DateTime(2024, 05, 15));
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-date")]
    [InlineData("2024-13-45")]
    public void ConvertToMovieDto_InvalidOrEmptyReleaseDateString_FallsBackToMinValue(string invalidDate)
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
        result.ReleaseDate.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public void ConvertToMovieDto_NullReleaseDateString_FallsBackToMinValue()
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
        result.ReleaseDate.Should().Be(DateTime.MinValue);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(120, 120)]
    [InlineData(185, 185)]
    public void ConvertToMovieDto_RuntimeMinutesProvided_ConvertsToDurationTimeSpan(int runtimeMinutes, int expectedMinutes)
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
        result.Duration.Should().Be(TimeSpan.FromMinutes(expectedMinutes));
    }

    [Fact]
    public void ConvertToMovieDto_PosterAndTrailerKeysProvided_GeneratesExpectedUrls()
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
        result.PosterURL.Should().Be("https://image.tmdb.org/t/p/w500/my_poster_image.png");
        result.TrailerURL.Should().Be("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }
}
