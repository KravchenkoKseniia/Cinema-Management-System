using Ardalis.Specification;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Entities.Specifications;
using Infrastructure.Interfaces;
using Moq;

namespace CinemaSystem.Test.Fixtures;

public sealed class SessionFilteringTests : IClassFixture<MapperFixture>, IDisposable
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IGenericRepository<Session>> _sessionRepo = new();
    private readonly SessionService _service;
    private ISpecification<Session>? _capturedSpec;

    public SessionFilteringTests(MapperFixture mapperFixture)
    {
        _unitOfWork.Setup(u => u.Sessions).Returns(_sessionRepo.Object);
        _service = new SessionService(_unitOfWork.Object, mapperFixture.Mapper);
    }

    public void Dispose()
    {
        _sessionRepo.VerifyAll();
    }
    
    // Claude helped here me with these 2 private methods to realize the "guardrail" for mutations
    private void RepositoryAnswers<TSpec>(params Session[] sessions)
        where TSpec : ISpecification<Session>
    {
        _sessionRepo.Setup(r => r.GetListBySpec(It.Is<ISpecification<Session>>(spec => spec is TSpec)))
                    .Callback<ISpecification<Session>>(spec => _capturedSpec = spec)
                    .Returns(sessions.ToList());
    }

    private IReadOnlyList<Session> ApplyCapturedSpecTo(params Session[] catalog)
    {
        Assert.NotNull(_capturedSpec);
        return _capturedSpec!.Evaluate(catalog).ToList();
    }

    [Fact]
    public void GetSessionsByMovie_SessionsOfOtherMovies_AreFilteredOut()
    {
        RepositoryAnswers<SessionsByMovieSpecification>();

        _service.GetSessionsByMovie(7).ToList();

        var visible = ApplyCapturedSpecTo(
            new Session { SessionId = 1, MovieId = 7 },
            new Session { SessionId = 2, MovieId = 9 });

        var session = Assert.Single(visible);
        Assert.Equal(1, session.SessionId);
    }

    [Fact]
    public void GetAllSessions_EntityFields_AreMappedToDto()
    {
        RepositoryAnswers<AllSessionsSpecification>(
            new Session
            {
                SessionId = 5,
                MovieId = 7,
                HallId = 3,
                Date = new DateTime(2026, 7, 12),
                StartTime = new TimeSpan(18, 0, 0),
                EndTime = new TimeSpan(20, 30, 0),
                TicketPrice = 150
            });

        var dto = Assert.Single(_service.GetAllSessions());

        Assert.Equal(5, dto.SessionId);
        Assert.Equal(7, dto.MovieId);
        Assert.Equal(3, dto.HallId);
        Assert.Equal(new DateTime(2026, 7, 12), dto.Date);
        Assert.Equal(new TimeSpan(18, 0, 0), dto.StartTime);
        Assert.Equal(new TimeSpan(20, 30, 0), dto.EndTime);
        Assert.Equal(150, dto.TicketPrice);
    }

    [Fact]
    public void GetAllSessions_NoSessionsExist_ReturnsEmptySequence()
    {
        RepositoryAnswers<AllSessionsSpecification>();

        var result = _service.GetAllSessions();

        Assert.Empty(result);
    }
}
