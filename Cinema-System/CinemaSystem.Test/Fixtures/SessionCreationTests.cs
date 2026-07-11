using AutoMapper;
using Cinema_System.DTOs;
using Cinema_System.Mapping;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Moq;

namespace CinemaSystem.Test.Fixtures;

public class MapperFixture : IDisposable
{
    public IMapper Mapper { get; }

    public MapperFixture()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        Mapper = configuration.CreateMapper();
    }

    public void Dispose()
    {
    }
}

public class SessionCreationTests : IClassFixture<MapperFixture>, IDisposable
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IGenericRepository<Session>> _sessionRepo = new();
    private readonly Mock<IGenericRepository<Movie>> _movieRepo = new();
    private readonly Mock<IGenericRepository<Hall>> _hallRepo = new();
    private readonly SessionService _service;

    public SessionCreationTests(MapperFixture mapperFixture)
    {
        _unitOfWork.Setup(u => u.Sessions).Returns(_sessionRepo.Object);
        _unitOfWork.Setup(u => u.Movies).Returns(_movieRepo.Object);
        _unitOfWork.Setup(u => u.Halls).Returns(_hallRepo.Object);

        _service = new SessionService(_unitOfWork.Object, mapperFixture.Mapper);
    }
    
    public void Dispose()
    {
        _sessionRepo.VerifyNoOtherCalls();
    }

    private static SessionDTO TestSessionRequest(
        string movieTitle = "Inception",
        string hallName = "Main Hall") => new()
    {
        MovieTitle = movieTitle,
        HallName = hallName,
        Date = new DateTime(2026, 7, 12),
        StartTime = new TimeSpan(18, 0, 0),
        EndTime = new TimeSpan(20, 30, 0),
        TicketPrice = 150
    };

    private void CatalogContains(params Movie[] movies) =>
        _movieRepo.Setup(r => r.GetAll()).Returns(movies.ToList());

    private void HallsContain(params Hall[] halls) =>
        _hallRepo.Setup(r => r.GetAll()).Returns(halls.ToList());

    [Fact]
    public void CreateSession_MovieDoesNotExist_SavesNothing()
    {
        CatalogContains(new Movie { MovieId = 7, Title = "Interstellar" });

        _service.CreateSession(TestSessionRequest(movieTitle: "Inception"));

        _sessionRepo.Verify(r => r.Insert(It.IsAny<Session>()), Times.Never);
        _unitOfWork.Verify(u => u.Save(), Times.Never);
    }

    [Fact]
    public void CreateSession_HallDoesNotExist_SavesNothing()
    {
        CatalogContains(new Movie { MovieId = 7, Title = "Inception" });
        HallsContain(new Hall { HallId = 3, Id = 3, Name = "Small Hall" });

        _service.CreateSession(TestSessionRequest(hallName: "Main Hall"));

        _sessionRepo.Verify(r => r.Insert(It.IsAny<Session>()), Times.Never);
        _unitOfWork.Verify(u => u.Save(), Times.Never);
    }

    [Fact]
    public void CreateSession_MovieTitleDiffersOnlyByCase_StillCreatesSession()
    {
        // "INCEPTION" and "Inception" are the same movie so the
        // session must be created. Currently fails - bug, which test should highlight, not build a doc on it
        CatalogContains(new Movie { MovieId = 7, Title = "Inception" });
        HallsContain(new Hall { HallId = 3, Id = 3, Name = "Main Hall" });

        _service.CreateSession(TestSessionRequest(movieTitle: "INCEPTION"));

        _sessionRepo.Verify(r => r.Insert(It.Is<Session>(s => s.MovieId == 7)), Times.Once);
        _unitOfWork.Verify(u => u.Save(), Times.Once);
    }
}
