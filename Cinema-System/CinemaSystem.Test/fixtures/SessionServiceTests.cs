using System;
using System.Collections.Generic;
using System.Linq;
using Cinema_System.DTOs;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Entities.Specifications;
using Infrastructure.Interfaces;
using Moq;
using FluentAssertions;

namespace CinemaSystem.Test.Services;

public class SessionServiceTests : IClassFixture<MapperFixture>
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IGenericRepository<Session>> _sessionRepoMock;
    private readonly Mock<IGenericRepository<Movie>> _movieRepoMock;
    private readonly Mock<IGenericRepository<Hall>> _hallRepoMock;
    private readonly MapperFixture _mapperFixture;
    private readonly SessionService _sessionService;

    public SessionServiceTests(MapperFixture mapperFixture)
    {
        _uowMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        _sessionRepoMock = new Mock<IGenericRepository<Session>>(MockBehavior.Strict);
        _movieRepoMock = new Mock<IGenericRepository<Movie>>(MockBehavior.Strict);
        _hallRepoMock = new Mock<IGenericRepository<Hall>>(MockBehavior.Strict);
        _mapperFixture = mapperFixture;

        _uowMock.Setup(uow => uow.Sessions).Returns(_sessionRepoMock.Object);
        _uowMock.Setup(uow => uow.Movies).Returns(_movieRepoMock.Object);
        _uowMock.Setup(uow => uow.Halls).Returns(_hallRepoMock.Object);

        _sessionService = new SessionService(_uowMock.Object, _mapperFixture.Mapper);
    }

    [Fact]
    public void GetAllSessions_SessionsExist_ReturnsMappedDtos()
    {
        // Arrange
        var sessions = new List<Session>
        {
            new Session { SessionId = 1, MovieId = 10, HallId = 5, TicketPrice = 12.5m }
        };

        _sessionRepoMock
            .Setup(r => r.GetListBySpec(It.IsAny<AllSessionsSpecification>()))
            .Returns(sessions);

        // Act
        var result = _sessionService.GetAllSessions();

        // Assert
        result.Should().HaveCount(1);
        result.First().SessionId.Should().Be(1);
        result.First().TicketPrice.Should().Be(12.5m);
    }

    [Fact]
    public void GetSessionsByMovie_SessionsExist_ReturnsMappedDtos()
    {
        // Arrange
        var sessions = new List<Session>
        {
            new Session { SessionId = 2, MovieId = 10, HallId = 5 }
        };

        _sessionRepoMock
            .Setup(r => r.GetListBySpec(It.IsAny<SessionsByMovieSpecification>()))
            .Returns(sessions);

        // Act
        var result = _sessionService.GetSessionsByMovie(10);

        // Assert
        result.Should().HaveCount(1);
        result.First().SessionId.Should().Be(2);
    }

    [Fact]
    public void GetSessionsByMovieAndDate_SessionsExist_ReturnsMappedDtos()
    {
        // Arrange
        var date = new DateTime(2026, 7, 12);
        var sessions = new List<Session>
        {
            new Session { SessionId = 3, MovieId = 10, HallId = 5, Date = date }
        };

        _sessionRepoMock
            .Setup(r => r.GetListBySpec(It.IsAny<SessionsByMovieAndDataSpecification>()))
            .Returns(sessions);

        // Act
        var result = _sessionService.GetSessionsByMovieAndDate(10, date);

        // Assert
        result.Should().HaveCount(1);
        result.First().SessionId.Should().Be(3);
    }

    [Fact]
    public void GetSessionById_SessionExists_ReturnsDtoWithMovieAndHallNames()
    {
        // Arrange
        var session = new Session { SessionId = 1, MovieId = 10, HallId = 5 };
        var movie = new Movie { MovieId = 10, Title = "Inception" };
        var hall = new Hall { HallId = 5, Name = "Main Room" };

        _sessionRepoMock.Setup(r => r.GetById(1)).Returns(session);
        _movieRepoMock.Setup(r => r.GetById(10)).Returns(movie);
        _hallRepoMock.Setup(r => r.GetById(5)).Returns(hall);

        // Act
        var result = _sessionService.GetSessionById(1);

        // Assert
        result.Should().NotBeNull();
        result!.SessionId.Should().Be(1);
        result.MovieTitle.Should().Be("Inception");
        result.HallName.Should().Be("Main Room");
    }

    [Fact]
    public void GetSessionById_SessionDoesNotExist_ReturnsNull()
    {
        // Arrange
        _sessionRepoMock.Setup(r => r.GetById(99)).Returns((Session)null!);

        // Act
        var result = _sessionService.GetSessionById(99);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetSessionById_MovieAndHallMissing_ReturnsDtoWithFallbackNames()
    {
        // Arrange
        var session = new Session { SessionId = 1, MovieId = 10, HallId = 5 };

        _sessionRepoMock.Setup(r => r.GetById(1)).Returns(session);
        _movieRepoMock.Setup(r => r.GetById(10)).Returns((Movie)null!);
        _hallRepoMock.Setup(r => r.GetById(5)).Returns((Hall)null!);

        // Act
        var result = _sessionService.GetSessionById(1);

        // Assert
        result.Should().NotBeNull();
        result!.MovieTitle.Should().Be("Unknown Movie");
        result.HallName.Should().Be("Unknown Hall");
    }

    [Fact]
    public void CreateSession_MovieAndHallExist_PersistsSession()
    {
        // Arrange
        var dto = new SessionDTO
        {
            MovieTitle = "Inception",
            HallName = "Main Hall",
            Date = new DateTime(2026, 7, 12),
            StartTime = new TimeSpan(14, 0, 0),
            EndTime = new TimeSpan(16, 30, 0),
            TicketPrice = 15.0m
        };

        var movie = new Movie { MovieId = 10, Title = "Inception" };
        var hall = new Hall { HallId = 5, Name = "Main Hall" };

        _movieRepoMock.Setup(r => r.GetAll()).Returns(new List<Movie> { movie });
        _hallRepoMock.Setup(r => r.GetAll()).Returns(new List<Hall> { hall });
        _sessionRepoMock.Setup(r => r.Insert(It.Is<Session>(s => 
            s.MovieId == 10 &&
            s.HallId == 5 &&
            s.TicketPrice == 15.0m
        )));
        _uowMock.Setup(uow => uow.Save()).Returns(1);

        // Act
        _sessionService.CreateSession(dto);

        // Assert
        _sessionRepoMock.Verify(r => r.Insert(It.IsAny<Session>()), Times.Once);
        _uowMock.Verify(uow => uow.Save(), Times.Once);
    }

    [Fact]
    public void CreateSession_MovieDoesNotExist_DoesNotPersistSession()
    {
        // SMELL: SessionService.CreateSession does Console.WriteLine + return on a missing movie instead of throwing/returning a result.
        
        // Arrange
        var dto = new SessionDTO
        {
            MovieTitle = "NonExistent",
            HallName = "Main Hall"
        };

        _movieRepoMock.Setup(r => r.GetAll()).Returns(new List<Movie>());

        // Act
        _sessionService.CreateSession(dto);

        // Assert
        _uowMock.Verify(uow => uow.Save(), Times.Never);
    }

    [Fact]
    public void CreateSession_HallDoesNotExist_DoesNotPersistSession()
    {
        // SMELL: SessionService.CreateSession does Console.WriteLine + return on a missing hall instead of throwing/returning a result.

        // Arrange
        var dto = new SessionDTO
        {
            MovieTitle = "Inception",
            HallName = "NonExistent"
        };

        var movie = new Movie { MovieId = 10, Title = "Inception" };

        _movieRepoMock.Setup(r => r.GetAll()).Returns(new List<Movie> { movie });
        _hallRepoMock.Setup(r => r.GetAll()).Returns(new List<Hall>());

        // Act
        _sessionService.CreateSession(dto);

        // Assert
        _uowMock.Verify(uow => uow.Save(), Times.Never);
    }

    [Fact]
    public void UpdateSession_SessionExists_UpdatesPropertiesAndSaves()
    {
        // Arrange
        var session = new Session
        {
            SessionId = 1,
            MovieId = 10,
            HallId = 5,
            Date = new DateTime(2026, 7, 12),
            StartTime = new TimeSpan(12, 0, 0),
            EndTime = new TimeSpan(14, 0, 0),
            TicketPrice = 10.0m
        };

        var dto = new SessionDTO
        {
            SessionId = 1,
            Date = new DateTime(2026, 7, 13),
            StartTime = new TimeSpan(15, 0, 0),
            EndTime = new TimeSpan(17, 0, 0),
            TicketPrice = 12.5m
        };

        _sessionRepoMock.Setup(r => r.GetById(1)).Returns(session);
        _sessionRepoMock.Setup(r => r.Update(session));
        _uowMock.Setup(uow => uow.Save()).Returns(1);

        // Act
        _sessionService.UpdateSession(dto);

        // Assert
        session.Date.Should().Be(new DateTime(2026, 7, 13));
        session.StartTime.Should().Be(new TimeSpan(15, 0, 0));
        session.EndTime.Should().Be(new TimeSpan(17, 0, 0));
        session.TicketPrice.Should().Be(12.5m);
        _sessionRepoMock.Verify(r => r.Update(session), Times.Once);
        _uowMock.Verify(uow => uow.Save(), Times.Once);
    }

    [Fact]
    public void UpdateSession_SessionDoesNotExist_DoesNotUpdateOrSave()
    {
        // Arrange
        var dto = new SessionDTO { SessionId = 99 };
        _sessionRepoMock.Setup(r => r.GetById(99)).Returns((Session)null!);

        // Act
        _sessionService.UpdateSession(dto);

        // Assert
        _uowMock.Verify(uow => uow.Save(), Times.Never);
    }

    [Fact]
    public void DeleteSession_IdProvided_DeletesSessionAndSaves()
    {
        // Arrange
        _sessionRepoMock.Setup(r => r.Delete(77));
        _uowMock.Setup(uow => uow.Save()).Returns(1);

        // Act
        _sessionService.DeleteSession(77);

        // Assert
        _sessionRepoMock.Verify(r => r.Delete(77), Times.Once);
        _uowMock.Verify(uow => uow.Save(), Times.Once);
    }
}
