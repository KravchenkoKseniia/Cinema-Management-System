using Cinema_System.Services.Interfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MVC.Controllers;
using Xunit;

namespace CinemaSystem.Test.Controllers
{
    public class PaymentsControllerTests
    {
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly PaymentsController _controller;

        public PaymentsControllerTests()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _controller = new PaymentsController(_paymentServiceMock.Object);
        }

        [Fact]
        public async Task GetPayments_ShouldReturnOkWithPayments()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new Payment { PaymentId = 1, UserId = 1, Amount = 100.00m },
                new Payment { PaymentId = 2, UserId = 2, Amount = 200.00m }
            };
            _paymentServiceMock.Setup(s => s.GetPaymentsAsync()).ReturnsAsync(payments);

            // Act
            var result = await _controller.GetPayments();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPayments = Assert.IsAssignableFrom<List<Payment>>(okResult.Value);
            Assert.Equal(2, returnedPayments.Count);
        }

        [Fact]
        public async Task GetUserPayments_ShouldReturnOkWithUserPayments()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new Payment { PaymentId = 1, UserId = 1, Amount = 100.00m }
            };
            _paymentServiceMock.Setup(s => s.GetUserPaymentsAsync(1)).ReturnsAsync(payments);

            // Act
            var result = await _controller.GetUserPayments(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPayments = Assert.IsAssignableFrom<List<Payment>>(okResult.Value);
            Assert.Single(returnedPayments);
            Assert.Equal(1, returnedPayments[0].UserId);
        }
    }
}
