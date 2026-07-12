using System;
using System.Collections.Generic;
using Cinema_System.DTOs;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Moq;
using FluentAssertions;

namespace CinemaSystem.Test.Services;

public class MovieServiceTests : IClassFixture<MapperFixture>
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IGenericRepository<Movie>> _movieRepoMock;
    private readonly MapperFixture _mapperFixture;
    private readonly MovieService _movieService;

    public MovieServiceTests(MapperFixture mapperFixture)
    {
        _uowMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        _movieRepoMock = new Mock<IGenericRepository<Movie>>(MockBehavior.Strict);
        _mapperFixture = mapperFixture;

        _uowMock.Setup(uow => uow.Movies).Returns(_movieRepoMock.Object);

        _movieService = new MovieService(_uowMock.Object, _mapperFixture.Mapper);
    }

    [Fact]
    public void GetAllMovies_MoviesExist_ReturnsMappedDtos()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { MovieId = 1, Title = "Dune", Description = "Sci-fi epic" },
            new Movie { MovieId = 2, Title = "Inception", Description = "Dream heist" }
        };

        _movieRepoMock.Setup(r => r.GetAll()).Returns(movies);

        // Act
        var result = _movieService.GetAllMovies();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(m => m.MovieId == 1 && m.Title == "Dune");
        result.Should().Contain(m => m.MovieId == 2 && m.Title == "Inception");
    }

    [Fact]
    public void GetAllMovies_NoMoviesExist_ReturnsEmptyCollection()
    {
        // Arrange
        _movieRepoMock.Setup(r => r.GetAll()).Returns(new List<Movie>());

        // Act
        var result = _movieService.GetAllMovies();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetMovieById_MovieExists_ReturnsMappedDto()
    {
        // Arrange
        var movie = new Movie { MovieId = 7, Title = "Avatar", Description = "Blue aliens" };
        _movieRepoMock.Setup(r => r.GetById(7)).Returns(movie);

        // Act
        var result = _movieService.GetMovieById(7);

        // Assert
        result.Should().NotBeNull();
        result!.MovieId.Should().Be(7);
        result.Title.Should().Be("Avatar");
        result.Description.Should().Be("Blue aliens");
    }

    [Fact]
    public void GetMovieById_MovieDoesNotExist_ReturnsNull()
    {
        // Arrange
        _movieRepoMock.Setup(r => r.GetById(99)).Returns((Movie)null!);

        // Act
        var result = _movieService.GetMovieById(99);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void CreateMovie_ValidMovieDto_MapsAndPersistsMovie()
    {
        // Arrange
        var movieDto = new MovieDTO
        {
            MovieId = 0,
            Title = "Interstellar",
            Description = "Wormholes",
            TrailerURL = "http://youtube.com/123",
            PosterURL = "http://image.tmdb/123",
            ReleaseDate = new DateTime(2014, 11, 7),
            Rating = 8.6m,
            Duration = TimeSpan.FromMinutes(169)
        };

        _movieRepoMock.Setup(r => r.Insert(It.Is<Movie>(m => 
            m.Title == "Interstellar" &&
            m.Description == "Wormholes" &&
            m.Rating == 8.6m &&
            m.Duration == TimeSpan.FromMinutes(169)
        )));
        _uowMock.Setup(uow => uow.Save()).Returns(1);

        // Act
        _movieService.CreateMovie(movieDto);

        // Assert
        _movieRepoMock.Verify(r => r.Insert(It.IsAny<Movie>()), Times.Once);
        _uowMock.Verify(uow => uow.Save(), Times.Once);
    }

    [Fact]
    public void UpdateMovie_MovieExists_MapsAndUpdatesMovie()
    {
        // Arrange
        var existingMovie = new Movie { MovieId = 1, Title = "Old Title", Description = "Old Desc" };
        var movieDto = new MovieDTO { MovieId = 1, Title = "New Title", Description = "New Desc" };

        _movieRepoMock.Setup(r => r.GetById(1)).Returns(existingMovie);
        _movieRepoMock.Setup(r => r.Update(existingMovie));
        _uowMock.Setup(uow => uow.Save()).Returns(1);

        // Act
        _movieService.UpdateMovie(movieDto);

        // Assert
        existingMovie.Title.Should().Be("New Title");
        existingMovie.Description.Should().Be("New Desc");
        _movieRepoMock.Verify(r => r.Update(existingMovie), Times.Once);
        _uowMock.Verify(uow => uow.Save(), Times.Once);
    }

    [Fact]
    public void UpdateMovie_MovieDoesNotExist_DoesNotUpdateOrSave()
    {
        // Arrange
        var movieDto = new MovieDTO { MovieId = 99, Title = "New Title" };
        _movieRepoMock.Setup(r => r.GetById(99)).Returns((Movie)null!);

        // Act
        _movieService.UpdateMovie(movieDto);

        // Assert
        _uowMock.Verify(uow => uow.Save(), Times.Never);
    }

    [Fact]
    public void DeleteMovie_IdProvided_DeletesMovieAndSaves()
    {
        // Arrange
        _movieRepoMock.Setup(r => r.Delete(77));
        _uowMock.Setup(uow => uow.Save()).Returns(1);

        // Act
        _movieService.DeleteMovie(77);

        // Assert
        _movieRepoMock.Verify(r => r.Delete(77), Times.Once);
        _uowMock.Verify(uow => uow.Save(), Times.Once);
    }

    [Fact]
    public void GetMovieIdByName_MovieExists_ReturnsMovieId()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { MovieId = 42, Title = "Inception" }
        };
        _movieRepoMock.Setup(r => r.GetAll()).Returns(movies);

        // Act
        var result = _movieService.GetMovieIdByName("inception");

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public void GetMovieIdByName_MovieDoesNotExist_ReturnsZero()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { MovieId = 42, Title = "Inception" }
        };
        _movieRepoMock.Setup(r => r.GetAll()).Returns(movies);

        // Act
        var result = _movieService.GetMovieIdByName("Dune");

        // Assert
        result.Should().Be(0);
    }
}
