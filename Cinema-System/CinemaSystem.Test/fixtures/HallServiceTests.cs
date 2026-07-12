using System;
using System.Collections.Generic;
using System.Linq;
using Cinema_System.DTOs;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Moq;
using FluentAssertions;

namespace CinemaSystem.Test.Services;

public class HallServiceTests : IClassFixture<MapperFixture>
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IGenericRepository<Hall>> _hallRepoMock;
    private readonly MapperFixture _mapperFixture;
    private readonly HallService _hallService;

    public HallServiceTests(MapperFixture mapperFixture)
    {
        _uowMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
        _hallRepoMock = new Mock<IGenericRepository<Hall>>(MockBehavior.Strict);
        _mapperFixture = mapperFixture;

        _uowMock.Setup(uow => uow.Halls).Returns(_hallRepoMock.Object);

        _hallService = new HallService(_uowMock.Object, _mapperFixture.Mapper);
    }

    [Fact]
    public void GetAllHalls_HallsExist_ReturnsMappedDtos()
    {
        // Arrange
        var halls = new List<Hall>
        {
            new Hall { HallId = 1, Name = "Hall A", Capacity = 120 },
            new Hall { HallId = 2, Name = "Hall B", Capacity = 225 }
        };

        _hallRepoMock.Setup(r => r.GetAll()).Returns(halls);

        // Act
        var result = _hallService.GetAllHalls();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(h => h.HallId == 1 && h.Name == "Hall A");
        result.Should().Contain(h => h.HallId == 2 && h.Name == "Hall B");
    }

    [Fact]
    public void GetAllHalls_NoHallsExist_ReturnsEmptyCollection()
    {
        // Arrange
        _hallRepoMock.Setup(r => r.GetAll()).Returns(new List<Hall>());

        // Act
        var result = _hallService.GetAllHalls();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetHallById_HallExists_ReturnsMappedDto()
    {
        // Arrange
        var hall = new Hall { HallId = 5, Name = "IMAX", Capacity = 400 };
        _hallRepoMock.Setup(r => r.GetById(5)).Returns(hall);

        // Act
        var result = _hallService.GetHallById(5);

        // Assert
        result.Should().NotBeNull();
        result!.HallId.Should().Be(5);
        result.Name.Should().Be("IMAX");
    }

    [Fact]
    public void GetHallById_HallDoesNotExist_ReturnsNull()
    {
        // Arrange
        _hallRepoMock.Setup(r => r.GetById(99)).Returns((Hall)null!);

        // Act
        var result = _hallService.GetHallById(99);

        // Assert
        result.Should().BeNull();
    }
}
