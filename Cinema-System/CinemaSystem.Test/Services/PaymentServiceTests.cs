using Cinema_System.Services;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CinemaSystem.Test.Services
{
    public class PaymentServiceTests
    {
        private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetPaymentsAsync_ShouldReturnAllPayments()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Payments.AddRange(new List<Payment>
                {
                    new Payment { PaymentId = 1, UserId = 1, Amount = 10.00m, Date = DateTime.Today },
                    new Payment { PaymentId = 2, UserId = 2, Amount = 20.00m, Date = DateTime.Today }
                });
                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new ApplicationDbContext(options))
            {
                var service = new PaymentService(context);
                var result = await service.GetPaymentsAsync();

                Assert.Equal(2, result.Count);
                Assert.Contains(result, p => p.PaymentId == 1);
                Assert.Contains(result, p => p.PaymentId == 2);
            }
        }

        [Fact]
        public async Task GetUserPaymentsAsync_ShouldReturnOnlyUserPayments()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Payments.AddRange(new List<Payment>
                {
                    new Payment { PaymentId = 1, UserId = 1, Amount = 10.00m, Date = DateTime.Today },
                    new Payment { PaymentId = 2, UserId = 2, Amount = 20.00m, Date = DateTime.Today },
                    new Payment { PaymentId = 3, UserId = 1, Amount = 15.50m, Date = DateTime.Today }
                });
                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new ApplicationDbContext(options))
            {
                var service = new PaymentService(context);
                var result = await service.GetUserPaymentsAsync(1);

                Assert.Equal(2, result.Count);
                Assert.All(result, p => Assert.Equal(1, p.UserId));
            }
        }
    }
}
