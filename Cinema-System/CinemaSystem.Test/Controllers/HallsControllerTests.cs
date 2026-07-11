using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Cinema_System.DTOs;
using Cinema_System.Services;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.Controllers;
using Xunit;

namespace CinemaSystem.Test.Controllers
{
    public class HallsControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IGenericRepository<Hall>> _mockHallRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly HallsController _controller;

        public HallsControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockHallRepo = new Mock<IGenericRepository<Hall>>();
            _mockUnitOfWork.Setup(u => u.Halls).Returns(_mockHallRepo.Object);
            _mockMapper = new Mock<IMapper>();

            var hallService = new HallService(_mockUnitOfWork.Object, _mockMapper.Object);
            _controller = new HallsController(hallService);
        }

        [Fact]
        public async Task GetHalls_ShouldReturnOk_WithListOfHalls()
        {
            // Arrange
            var halls = new List<Hall>
            {
                new Hall { HallId = 1, Id = 1, Name = "Hall A", Capacity = 100 }
            };
            var hallDtos = new List<HallDTO>
            {
                new HallDTO { HallId = 1, Name = "Hall A", Capacity = 100 }
            };

            _mockHallRepo.Setup(r => r.GetAll()).Returns(halls);
            _mockMapper.Setup(m => m.Map<IEnumerable<HallDTO>>(halls)).Returns(hallDtos);

            // Act
            var result = await _controller.GetHalls();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedHalls = Assert.IsAssignableFrom<IEnumerable<HallDTO>>(okResult.Value);
            Assert.Single(returnedHalls);
        }

        [Fact]
        public async Task GetHallById_ShouldReturnOk_WhenHallExists()
        {
            // Arrange
            var hall = new Hall { HallId = 1, Id = 1, Name = "Hall A", Capacity = 100 };
            var hallDto = new HallDTO { HallId = 1, Name = "Hall A", Capacity = 100 };

            _mockHallRepo.Setup(r => r.GetById(1)).Returns(hall);
            _mockMapper.Setup(m => m.Map<HallDTO>(hall)).Returns(hallDto);

            // Act
            var result = await _controller.GetHallById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedHall = Assert.IsType<HallDTO>(okResult.Value);
            Assert.Equal("Hall A", returnedHall.Name);
        }

        [Fact]
        public async Task GetHallById_ShouldReturnNotFound_WhenHallDoesNotExist()
        {
            // Arrange
            _mockHallRepo.Setup(r => r.GetById(99)).Returns((Hall?)null);

            // Act
            var result = await _controller.GetHallById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
