using AutoMapper;
using Cinema_System.DTOs;
using Cinema_System.Mapping;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Moq;
using Ardalis.Specification;

namespace CinemaSystem.Test.Services
{
    public class SessionServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Session>> _mockSessionRepo;
        private readonly Mock<IGenericRepository<Movie>> _mockMovieRepo;
        private readonly Mock<IGenericRepository<Hall>> _mockHallRepo;
        private readonly IMapper _mapper;
        private readonly SessionService _sessionService;

        public SessionServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockSessionRepo = new Mock<IGenericRepository<Session>>();
            _mockMovieRepo = new Mock<IGenericRepository<Movie>>();
            _mockHallRepo = new Mock<IGenericRepository<Hall>>();

            // Setup UnitOfWork to return the mocked repositories
            _mockUnitOfWork.Setup(u => u.Sessions).Returns(_mockSessionRepo.Object);
            _mockUnitOfWork.Setup(u => u.Movies).Returns(_mockMovieRepo.Object);
            _mockUnitOfWork.Setup(u => u.Halls).Returns(_mockHallRepo.Object);

            // Configure real Mapper with real MappingProfile
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mapperConfig.CreateMapper();

            _sessionService = new SessionService(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public void GetAllSessions_ShouldReturnMappedSessions()
        {
            // Arrange
            var sessions = new List<Session>
            {
                new Session { SessionId = 1, MovieId = 10, HallId = 2, TicketPrice = 10.0m },
                new Session { SessionId = 2, MovieId = 11, HallId = 3, TicketPrice = 15.0m }
            };

            _mockSessionRepo.Setup(r => r.GetListBySpec(It.IsAny<ISpecification<Session>>()))
                            .Returns(sessions);

            // Act
            var result = _sessionService.GetAllSessions().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].SessionId);
            Assert.Equal(10.0m, result[0].TicketPrice);
            Assert.Equal(2, result[1].SessionId);
            Assert.Equal(15.0m, result[1].TicketPrice);
        }

        [Fact]
        public void GetSessionsByMovieAndDate_ShouldReturnMappedSessions()
        {
            // Arrange
            var sessions = new List<Session>
            {
                new Session { SessionId = 1, MovieId = 10, HallId = 2, Date = DateTime.Today }
            };

            _mockSessionRepo.Setup(r => r.GetListBySpec(It.IsAny<ISpecification<Session>>()))
                            .Returns(sessions);

            // Act
            var result = _sessionService.GetSessionsByMovieAndDate(10, DateTime.Today).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].SessionId);
            Assert.Equal(10, result[0].MovieId);
        }

        [Fact]
        public void GetSessionsByMovie_ShouldReturnMappedSessions()
        {
            // Arrange
            var sessions = new List<Session>
            {
                new Session { SessionId = 1, MovieId = 10, HallId = 2 }
            };

            _mockSessionRepo.Setup(r => r.GetListBySpec(It.IsAny<ISpecification<Session>>()))
                            .Returns(sessions);

            // Act
            var result = _sessionService.GetSessionsByMovie(10).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].SessionId);
            Assert.Equal(10, result[0].MovieId);
        }

        [Fact]
        public void GetSessionById_WhenSessionDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _mockSessionRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns((Session?)null);

            // Act
            var result = _sessionService.GetSessionById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetSessionById_WhenSessionExistsWithMovieAndHall_ShouldReturnFullDTO()
        {
            // Arrange
            var session = new Session { SessionId = 1, MovieId = 10, HallId = 2, TicketPrice = 12.0m };
            var movie = new Movie { MovieId = 10, Title = "Inception" };
            var hall = new Hall { HallId = 2, Name = "Hall A" };

            _mockSessionRepo.Setup(r => r.GetById(1)).Returns(session);
            _mockMovieRepo.Setup(r => r.GetById(10)).Returns(movie);
            _mockHallRepo.Setup(r => r.GetById(2)).Returns(hall);

            // Act
            var result = _sessionService.GetSessionById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.SessionId);
            Assert.Equal("Inception", result.MovieTitle);
            Assert.Equal("Hall A", result.HallName);
        }

        [Fact]
        public void GetSessionById_WhenMovieAndHallAreMissing_ShouldFallbackToUnknown()
        {
            // Arrange
            var session = new Session { SessionId = 1, MovieId = 10, HallId = 2 };

            _mockSessionRepo.Setup(r => r.GetById(1)).Returns(session);
            _mockMovieRepo.Setup(r => r.GetById(10)).Returns((Movie?)null);
            _mockHallRepo.Setup(r => r.GetById(2)).Returns((Hall?)null);

            // Act
            var result = _sessionService.GetSessionById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Unknown Movie", result.MovieTitle);
            Assert.Equal("Unknown Hall", result.HallName);
        }

        [Fact]
        public void CreateSession_WhenMovieDoesNotExist_ShouldNotInsertOrSave()
        {
            // Arrange
            var sessionDto = new SessionDTO { MovieTitle = "NonExistentMovie", HallName = "Hall A" };
            _mockMovieRepo.Setup(r => r.GetAll()).Returns(new List<Movie>());

            // Act
            _sessionService.CreateSession(sessionDto);

            // Assert
            _mockSessionRepo.Verify(r => r.Insert(It.IsAny<Session>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public void CreateSession_WhenHallDoesNotExist_ShouldNotInsertOrSave()
        {
            // Arrange
            var sessionDto = new SessionDTO { MovieTitle = "Inception", HallName = "NonExistentHall" };
            var movie = new Movie { MovieId = 10, Title = "Inception" };

            _mockMovieRepo.Setup(r => r.GetAll()).Returns(new List<Movie> { movie });
            _mockHallRepo.Setup(r => r.GetAll()).Returns(new List<Hall>());

            // Act
            _sessionService.CreateSession(sessionDto);

            // Assert
            _mockSessionRepo.Verify(r => r.Insert(It.IsAny<Session>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public void CreateSession_WhenMovieAndHallExist_ShouldInsertAndSave()
        {
            // Arrange
            var sessionDto = new SessionDTO
            {
                MovieTitle = "Inception",
                HallName = "Hall A",
                Date = DateTime.Today,
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(12, 0, 0),
                TicketPrice = 10.0m
            };
            var movie = new Movie { MovieId = 10, Title = "Inception" };
            var hall = new Hall { HallId = 2, Name = "Hall A" };

            _mockMovieRepo.Setup(r => r.GetAll()).Returns(new List<Movie> { movie });
            _mockHallRepo.Setup(r => r.GetAll()).Returns(new List<Hall> { hall });

            // Act
            _sessionService.CreateSession(sessionDto);

            // Assert
            _mockSessionRepo.Verify(r => r.Insert(It.Is<Session>(s =>
                s.MovieId == 10 &&
                s.HallId == 2 &&
                s.Date == sessionDto.Date &&
                s.StartTime == sessionDto.StartTime &&
                s.EndTime == sessionDto.EndTime &&
                s.TicketPrice == 10.0m
            )), Times.Once);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void UpdateSession_WhenSessionDoesNotExist_ShouldNotUpdateOrSave()
        {
            // Arrange
            var sessionDto = new SessionDTO { SessionId = 1 };
            _mockSessionRepo.Setup(r => r.GetById(1)).Returns((Session?)null);

            // Act
            _sessionService.UpdateSession(sessionDto);

            // Assert
            _mockSessionRepo.Verify(r => r.Update(It.IsAny<Session>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public void UpdateSession_WhenSessionExists_ShouldUpdateFieldsAndSave()
        {
            // Arrange
            var existingSession = new Session
            {
                SessionId = 1,
                MovieId = 10,
                HallId = 2,
                Date = DateTime.Today,
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(12, 0, 0),
                TicketPrice = 10.0m
            };

            var sessionDto = new SessionDTO
            {
                SessionId = 1,
                Date = DateTime.Today.AddDays(1),
                StartTime = new TimeSpan(14, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
                TicketPrice = 15.0m
            };

            _mockSessionRepo.Setup(r => r.GetById(1)).Returns(existingSession);

            // Act
            _sessionService.UpdateSession(sessionDto);

            // Assert
            Assert.Equal(sessionDto.Date, existingSession.Date);
            Assert.Equal(sessionDto.StartTime, existingSession.StartTime);
            Assert.Equal(sessionDto.EndTime, existingSession.EndTime);
            Assert.Equal(sessionDto.TicketPrice, existingSession.TicketPrice);

            _mockSessionRepo.Verify(r => r.Update(existingSession), Times.Once);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void DeleteSession_ShouldCallDeleteAndSave()
        {
            // Arrange
            int sessionId = 1;

            // Act
            _sessionService.DeleteSession(sessionId);

            // Assert
            _mockSessionRepo.Verify(r => r.Delete(sessionId), Times.Once);
            _mockUnitOfWork.Verify(u => u.Save(), Times.Once);
        }
    }
}
