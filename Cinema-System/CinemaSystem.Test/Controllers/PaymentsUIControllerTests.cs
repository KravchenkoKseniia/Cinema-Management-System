using System.Security.Claims;
using Cinema_System.Services.Interfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.ControllersUI;
using Xunit;

namespace CinemaSystem.Test.Controllers
{
    public class PaymentsUIControllerTests
    {
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly PaymentsUIController _controller;

        public PaymentsUIControllerTests()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _controller = new PaymentsUIController(_paymentServiceMock.Object);
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithPayments()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new Payment { PaymentId = 1, UserId = 1, Amount = 100.00m }
            };
            _paymentServiceMock.Setup(s => s.GetPaymentsAsync()).ReturnsAsync(payments);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Payment>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ShouldReturnBadRequest_WhenUserIdClaimIsMissingOrInvalid()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.Details();

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Details_ShouldReturnViewWithUserPayments_WhenUserIdIsValid()
        {
            // Arrange
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "42") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var payments = new List<Payment>
            {
                new Payment { PaymentId = 1, UserId = 42, Amount = 50.00m }
            };
            _paymentServiceMock.Setup(s => s.GetUserPaymentsAsync(42)).ReturnsAsync(payments);

            // Act
            var result = await _controller.Details();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Payment>>(viewResult.Model);
            Assert.Single(model);
            Assert.Equal(42, model[0].UserId);
        }
    }
}
