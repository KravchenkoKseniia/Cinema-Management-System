using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema_System.Constants;
using Cinema_System.DTOs;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Entities.Specifications;
using Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace CinemaSystem.Test.Services
{
    public class TmdbServiceTests
    {
        private readonly Mock<ITmdbRepository> _tmdbRepoMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IGenericRepository<Genre>> _genreRepoMock;
        private readonly TmdbService _tmdbService;

        public TmdbServiceTests()
        {
            _tmdbRepoMock = new Mock<ITmdbRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            _genreRepoMock = new Mock<IGenericRepository<Genre>>();

            _uowMock.Setup(uow => uow.Genres).Returns(_genreRepoMock.Object);

            _tmdbService = new TmdbService(_tmdbRepoMock.Object, _uowMock.Object);
        }

        [Fact]
        public async Task SearchMoviesByTitleAsync_ValidTitle_ShouldCallRepositoryAndReturnList()
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

            _tmdbRepoMock
                .Setup(repo => repo.FetchFromTmdbAsync<MovieSearchResultDTO>(TmdbEndpoints.SearchByTitleEndpoint(movieTitle)))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _tmdbService.SearchMoviesByTitleAsync(movieTitle);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Results.Count);
            
            _tmdbRepoMock.Verify(repo => repo.FetchFromTmdbAsync<MovieSearchResultDTO>(TmdbEndpoints.SearchByTitleEndpoint(movieTitle)), Times.Once);
        }

        [Fact]
        public async Task SearchMoviesByTitleAsync_WhenRepositoryReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var movieTitle = "NonExistentMovie123";
            
            _tmdbRepoMock
                .Setup(repo => repo.FetchFromTmdbAsync<MovieSearchResultDTO>(TmdbEndpoints.SearchByTitleEndpoint(movieTitle)))
                .ReturnsAsync((MovieSearchResultDTO)null!);

            // Act
            var result = await _tmdbService.SearchMoviesByTitleAsync(movieTitle);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task FetchMovieFromTmdbByIdAsync_ValidId_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            var movieId = 550;
            var expectedMovie = new MovieSearchItemDTO 
            { 
                Id = movieId, 
                Title = "Fight Club",
                Overview = "An insomniac office worker...",
                ReleaseDateString = "1999-10-15",
                PosterPath = "/fightclub.jpg"
            };

            _tmdbRepoMock
                .Setup(repo => repo.FetchFromTmdbAsync<MovieSearchItemDTO>(TmdbEndpoints.DetailsByIdEndpoint(movieId)))
                .ReturnsAsync(expectedMovie);

            _genreRepoMock
                .Setup(repo => repo.GetListBySpec(It.IsAny<AllGenresSpecification>()))
                .Returns(new List<Genre>());

            _tmdbRepoMock
                .Setup(repo => repo.FetchFromTmdbAsync<MovieTrailerResult>(TmdbEndpoints.TrailerKeyByIdEndpoint(movieId)))
                .ReturnsAsync(new MovieTrailerResult { Results = new List<MovieTrailerItem>() });

            // Act
            var result = await _tmdbService.FetchMovieFromTmdbByIdAsync(movieId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(movieId, result.MovieId);
            Assert.Equal("Fight Club", result.Title);
            
            _tmdbRepoMock.Verify(repo => repo.FetchFromTmdbAsync<MovieSearchItemDTO>(TmdbEndpoints.DetailsByIdEndpoint(movieId)), Times.Once);
        }
    }
}