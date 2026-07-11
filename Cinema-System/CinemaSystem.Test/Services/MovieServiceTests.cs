using AutoMapper;
using Cinema_System.DTOs;
using Cinema_System.Mapping;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Moq;

namespace CinemaSystem.Test.Services;

public class MovieServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGenericRepository<Movie>> _mockMovieRepo;
    private readonly IMapper _mapper;
    private readonly MovieService _service;

    public MovieServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMovieRepo = new Mock<IGenericRepository<Movie>>();
        _mockUnitOfWork.Setup(u => u.Movies).Returns(_mockMovieRepo.Object);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = config.CreateMapper();

        _service = new MovieService(_mockUnitOfWork.Object, _mapper);
    }

    [Fact]
    public void GetAllMovies_ShouldReturnMappedMovies()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { MovieId = 1, Title = "Movie 1" },
            new Movie { MovieId = 2, Title = "Movie 2" }
        };
        _mockMovieRepo.Setup(r => r.GetAll()).Returns(movies);

        // Act
        var result = _service.GetAllMovies().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Movie 1", result[0].Title);
        Assert.Equal("Movie 2", result[1].Title);
    }

    [Fact]
    public void GetMovieById_ShouldReturnNull_WhenMovieDoesNotExist()
    {
        // Arrange
        _mockMovieRepo.Setup(r => r.GetById(1)).Returns((Movie?)null);

        // Act
        var result = _service.GetMovieById(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetMovieById_ShouldReturnMappedMovie_WhenMovieExists()
    {
        // Arrange
        var movie = new Movie { MovieId = 1, Title = "Movie 1" };
        _mockMovieRepo.Setup(r => r.GetById(1)).Returns(movie);

        // Act
        var result = _service.GetMovieById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Movie 1", result.Title);
    }

    [Fact]
    public void CreateMovie_ShouldInsertAndSave()
    {
        // Arrange
        var dto = new MovieDTO { MovieId = 1, Title = "New Movie" };

        // Act
        _service.CreateMovie(dto);

        // Assert
        _mockMovieRepo.Verify(r => r.Insert(It.Is<Movie>(m => m.Title == "New Movie")), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }

    [Fact]
    public void UpdateMovie_ShouldUpdateAndSave_WhenMovieExists()
    {
        // Arrange
        var existingMovie = new Movie { MovieId = 1, Title = "Old Title" };
        var dto = new MovieDTO { MovieId = 1, Title = "New Title" };
        _mockMovieRepo.Setup(r => r.GetById(1)).Returns(existingMovie);

        // Act
        _service.UpdateMovie(dto);

        // Assert
        _mockMovieRepo.Verify(r => r.Update(It.Is<Movie>(m => m.Title == "New Title")), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }

    [Fact]
    public void UpdateMovie_ShouldNotUpdate_WhenMovieDoesNotExist()
    {
        // Arrange
        var dto = new MovieDTO { MovieId = 1, Title = "New Title" };
        _mockMovieRepo.Setup(r => r.GetById(1)).Returns((Movie?)null);

        // Act
        _service.UpdateMovie(dto);

        // Assert
        _mockMovieRepo.Verify(r => r.Update(It.IsAny<Movie>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
    }

    [Fact]
    public void DeleteMovie_ShouldDeleteAndSave()
    {
        // Act
        _service.DeleteMovie(1);

        // Assert
        _mockMovieRepo.Verify(r => r.Delete(1), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
    }

    [Fact]
    public void GetMovieIdByName_ShouldReturnZero_WhenMovieNotFound()
    {
        // Arrange
        _mockMovieRepo.Setup(r => r.GetAll()).Returns(new List<Movie>());

        // Act
        var result = _service.GetMovieIdByName("Nonexistent");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetMovieIdByName_ShouldReturnId_WhenMovieFound()
    {
        // Arrange
        var movies = new List<Movie> { new Movie { MovieId = 42, Title = "Expected Movie" } };
        _mockMovieRepo.Setup(r => r.GetAll()).Returns(movies);

        // Act
        var result = _service.GetMovieIdByName("Expected Movie");

        // Assert
        Assert.Equal(42, result);
    }
}
