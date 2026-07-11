using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Cinema_System.DTOs;
using Cinema_System.Mapping;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace CinemaSystem.Test.Services
{
    public class HallServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Hall>> _mockHallRepo;
        private readonly IMapper _mapper;
        private readonly HallService _service;

        public HallServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockHallRepo = new Mock<IGenericRepository<Hall>>();
            _mockUnitOfWork.Setup(u => u.Halls).Returns(_mockHallRepo.Object);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _service = new HallService(_mockUnitOfWork.Object, _mapper);
        }

        [Fact]
        public void GetAllHalls_ShouldReturnAllMappedHalls()
        {
            // Arrange
            var halls = new List<Hall>
            {
                new Hall { HallId = 1, Id = 1, Name = "Hall A", Capacity = 100 },
                new Hall { HallId = 2, Id = 2, Name = "Hall B", Capacity = 150 }
            };
            _mockHallRepo.Setup(r => r.GetAll()).Returns(halls);

            // Act
            var result = _service.GetAllHalls().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].HallId);
            Assert.Equal("Hall A", result[0].Name);
            Assert.Equal(100, result[0].Capacity);
            Assert.Equal(2, result[1].HallId);
            Assert.Equal("Hall B", result[1].Name);
            Assert.Equal(150, result[1].Capacity);
        }

        [Fact]
        public void GetHallById_ShouldReturnMappedHall_WhenExists()
        {
            // Arrange
            var hall = new Hall { HallId = 1, Id = 1, Name = "Hall A", Capacity = 100 };
            _mockHallRepo.Setup(r => r.GetById(1)).Returns(hall);

            // Act
            var result = _service.GetHallById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.HallId);
            Assert.Equal("Hall A", result.Name);
            Assert.Equal(100, result.Capacity);
        }

        [Fact]
        public void GetHallById_ShouldReturnNull_WhenDoesNotExist()
        {
            // Arrange
            _mockHallRepo.Setup(r => r.GetById(99)).Returns((Hall?)null);

            // Act
            var result = _service.GetHallById(99);

            // Assert
            Assert.Null(result);
        }
    }
}
