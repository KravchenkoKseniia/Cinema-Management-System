using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema_System.Constants;
using Cinema_System.DTOs;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Entities.Specifications;
using Infrastructure.Interfaces;
using Moq;
using FluentAssertions;

namespace CinemaSystem.Test.Services;

public class TmdbServiceTests
{
    private readonly Mock<ITmdbRepository> _tmdbRepoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IGenericRepository<Genre>> _genreRepoMock;
    private readonly TmdbService _tmdbService;

    public TmdbServiceTests()
    {
        _tmdbRepoMock = new Mock<ITmdbRepository>(MockBehavior.Strict);
        _uowMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        _genreRepoMock = new Mock<IGenericRepository<Genre>>(MockBehavior.Strict);

        _uowMock.Setup(uow => uow.Genres).Returns(_genreRepoMock.Object);

        _tmdbService = new TmdbService(_tmdbRepoMock.Object, _uowMock.Object);
    }

    [Fact]
    public async Task SearchMoviesByTitleAsync_ValidTitle_CallsRepositoryAndReturnsResults()
    {
        // Arrange
        var movieTitle = "Dune";
        var expectedResponse = new MovieSearchResultDTO
        {
            Results = new List<MovieSearchItemDTO>
            {
                new MovieSearchItemDTO { Id = 1, Title = "Dune: Part One" },
                new MovieSearchItemDTO { Id = 2, Title = "Dune: Part Two" }
            }
        };

        var expectedUrl = TmdbEndpoints.SearchByTitleEndpoint("Dune");
        _tmdbRepoMock
            .Setup(repo => repo.FetchFromTmdbAsync<MovieSearchResultDTO>(expectedUrl))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _tmdbService.SearchMoviesByTitleAsync(movieTitle);

        // Assert
        result.Should().NotBeNull();
        result!.Results.Should().HaveCount(2);
        result.Results[0].Title.Should().Be("Dune: Part One");
        result.Results[1].Title.Should().Be("Dune: Part Two");
        
        _tmdbRepoMock.Verify(repo => repo.FetchFromTmdbAsync<MovieSearchResultDTO>(expectedUrl), Times.Once);
    }

    [Fact]
    public async Task SearchMoviesByTitleAsync_RepositoryReturnsNull_ReturnsNull()
    {
        // Arrange
        var movieTitle = "NonExistentMovie123";
        var expectedUrl = TmdbEndpoints.SearchByTitleEndpoint("NonExistentMovie123");
        
        _tmdbRepoMock
            .Setup(repo => repo.FetchFromTmdbAsync<MovieSearchResultDTO>(expectedUrl))
            .ReturnsAsync((MovieSearchResultDTO)null!);

        // Act
        var result = await _tmdbService.SearchMoviesByTitleAsync(movieTitle);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task FetchMovieFromTmdbByIdAsync_ValidId_CallsRepositoryAndReturnsMappedMovie()
    {
        // Arrange
        var movieId = 550;
        var expectedMovie = new MovieSearchItemDTO 
        { 
            Id = 550, 
            Title = "Fight Club",
            Overview = "An insomniac office worker...",
            ReleaseDateString = "1999-10-15",
            PosterPath = "/fightclub.jpg"
        };

        var detailsUrl = TmdbEndpoints.DetailsByIdEndpoint(550);
        var trailerUrl = TmdbEndpoints.TrailerKeyByIdEndpoint(550);

        _tmdbRepoMock
            .Setup(repo => repo.FetchFromTmdbAsync<MovieSearchItemDTO>(detailsUrl))
            .ReturnsAsync(expectedMovie);

        _genreRepoMock
            .Setup(repo => repo.GetListBySpec(It.IsAny<AllGenresSpecification>()))
            .Returns(new List<Genre>());

        _tmdbRepoMock
            .Setup(repo => repo.FetchFromTmdbAsync<MovieTrailerResult>(trailerUrl))
            .ReturnsAsync(new MovieTrailerResult { Results = new List<MovieTrailerItem>() });

        // Act
        var result = await _tmdbService.FetchMovieFromTmdbByIdAsync(movieId);

        // Assert
        result.Should().NotBeNull();
        result!.MovieId.Should().Be(550);
        result.Title.Should().Be("Fight Club");
        
        _tmdbRepoMock.Verify(repo => repo.FetchFromTmdbAsync<MovieSearchItemDTO>(detailsUrl), Times.Once);
    }
}
