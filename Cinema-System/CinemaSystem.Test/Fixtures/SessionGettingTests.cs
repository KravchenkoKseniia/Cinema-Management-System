using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Moq;

namespace CinemaSystem.Test.Fixtures;

public sealed class SessionGettingTests : IClassFixture<MapperFixture>, IDisposable
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IGenericRepository<Session>> _sessionRepo = new();
    private readonly Mock<IGenericRepository<Movie>> _movieRepo = new();
    private readonly Mock<IGenericRepository<Hall>> _hallRepo = new();
    private readonly SessionService _service;

    public SessionGettingTests(MapperFixture mapperFixture)
    {
        _unitOfWork.Setup(u => u.Sessions).Returns(_sessionRepo.Object);
        _unitOfWork.Setup(u => u.Movies).Returns(_movieRepo.Object);
        _unitOfWork.Setup(u => u.Halls).Returns(_hallRepo.Object);

        _service = new SessionService(_unitOfWork.Object, mapperFixture.Mapper);
    }

    public void Dispose()
    {
        _sessionRepo.VerifyAll();
        _movieRepo.VerifyAll();
        _hallRepo.VerifyAll();
    }

    private void TestSession(int sessionId = 5, int movieId = 7, int hallId = 3) =>
        _sessionRepo.Setup(r => r.GetById(sessionId)).Returns(new Session
        {
            SessionId = sessionId,
            MovieId = movieId,
            HallId = hallId,
            Date = new DateTime(2026, 7, 12),
            StartTime = new TimeSpan(18, 0, 0),
            EndTime = new TimeSpan(20, 30, 0),
            TicketPrice = 150
        });

    [Fact]
    public void GetSessionById_UnknownSession_ReturnsNullWithoutTouchingOtherRepositories()
    {
        _sessionRepo.Setup(r => r.GetById(42)).Returns((Session?)null);

        var result = _service.GetSessionById(42);

        Assert.Null(result);
        _movieRepo.VerifyNoOtherCalls();
        _hallRepo.VerifyNoOtherCalls();
    }

    [Fact]
    public void GetSessionById_ExistingSession_MapsScheduleFieldsToDto()
    {
        TestSession();
        _movieRepo.Setup(r => r.GetById(7)).Returns(new Movie { MovieId = 7, Title = "Inception" });
        _hallRepo.Setup(r => r.GetById(3)).Returns(new Hall { HallId = 3, Id = 3, Name = "Main Hall" });

        var dto = _service.GetSessionById(5);

        Assert.NotNull(dto);
        Assert.Equal(5, dto.SessionId);
        Assert.Equal(new DateTime(2026, 7, 12), dto.Date);
        Assert.Equal(new TimeSpan(18, 0, 0), dto.StartTime);
        Assert.Equal(new TimeSpan(20, 30, 0), dto.EndTime);
        Assert.Equal(150, dto.TicketPrice);
    }

    [Fact]
    public void GetSessionById_ExistingSession_HaveMovieTitleAndHallName()
    {
        TestSession();
        _movieRepo.Setup(r => r.GetById(7)).Returns(new Movie { MovieId = 7, Title = "Inception" });
        _hallRepo.Setup(r => r.GetById(3)).Returns(new Hall { HallId = 3, Id = 3, Name = "Main Hall" });

        var dto = _service.GetSessionById(5);

        Assert.NotNull(dto);
        Assert.Equal("Inception", dto.MovieTitle);
        Assert.Equal("Main Hall", dto.HallName);
    }
}
