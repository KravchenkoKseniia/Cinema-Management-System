using System.Collections.Generic;
using System.Threading.Tasks;
using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.ControllersUI;
using Xunit;

namespace CinemaSystem.Test.Controllers
{
    public class HallsUIControllerTests
    {
        private readonly Mock<IHallService> _mockHallService;
        private readonly HallsUIController _controller;

        public HallsUIControllerTests()
        {
            _mockHallService = new Mock<IHallService>();
            _controller = new HallsUIController(_mockHallService.Object);
        }

        [Fact]
        public async Task Index_ShouldReturnView_WithListOfHalls()
        {
            // Arrange
            var hallDtos = new List<HallDTO>
            {
                new HallDTO { HallId = 1, Name = "Hall A", Capacity = 100 },
                new HallDTO { HallId = 2, Name = "Hall B", Capacity = 150 }
            };
            _mockHallService.Setup(s => s.GetAllHalls()).Returns(hallDtos);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedHalls = Assert.IsAssignableFrom<IEnumerable<HallDTO>>(viewResult.Model);
            Assert.Equal(2, ((List<HallDTO>)returnedHalls).Count);
        }

        [Fact]
        public async Task Details_ShouldReturnView_WhenHallExists()
        {
            // Arrange
            var hallDto = new HallDTO { HallId = 1, Name = "Hall A", Capacity = 100 };
            _mockHallService.Setup(s => s.GetHallById(1)).Returns(hallDto);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedHall = Assert.IsType<HallDTO>(viewResult.Model);
            Assert.Equal("Hall A", returnedHall.Name);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenHallDoesNotExist()
        {
            // Arrange
            _mockHallService.Setup(s => s.GetHallById(99)).Returns((HallDTO?)null);

            // Act
            var result = await _controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
