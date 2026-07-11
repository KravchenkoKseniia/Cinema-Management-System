using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cinema_System.Services;
using Infrastructure.Data;
using Infrastructure.Entities;
using FluentAssertions;

namespace CinemaSystem.Test.Services;

// NOTE: This is a narrow integration test using an In-Memory Database as specified in the test strategy.
[Trait("Category", "Integration")]
public class PaymentServiceTests
{
    private ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task GetPaymentsAsync_PaymentsExist_ReturnsAllPayments()
    {
        // Arrange
        using var context = CreateDbContext();
        
        // Remove seed data if any exists to keep data set clean
        context.Payments.RemoveRange(context.Payments);
        await context.SaveChangesAsync();

        var payment1 = new Payment { PaymentId = 1, UserId = 10, Amount = 15.5m };
        var payment2 = new Payment { PaymentId = 2, UserId = 20, Amount = 20.0m };
        context.Payments.AddRange(payment1, payment2);
        await context.SaveChangesAsync();

        var service = new PaymentService(context);

        // Act
        var result = await service.GetPaymentsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.PaymentId == 1 && p.UserId == 10 && p.Amount == 15.5m);
        result.Should().Contain(p => p.PaymentId == 2 && p.UserId == 20 && p.Amount == 20.0m);
    }

    [Fact]
    public async Task GetPaymentsAsync_NoPaymentsExist_ReturnsEmptyList()
    {
        // Arrange
        using var context = CreateDbContext();
        context.Payments.RemoveRange(context.Payments);
        await context.SaveChangesAsync();

        var service = new PaymentService(context);

        // Act
        var result = await service.GetPaymentsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUserPaymentsAsync_UserHasPayments_ReturnsUserPaymentsOnly()
    {
        // Arrange
        using var context = CreateDbContext();
        context.Payments.RemoveRange(context.Payments);

        var payment1 = new Payment { PaymentId = 1, UserId = 10, Amount = 15.5m };
        var payment2 = new Payment { PaymentId = 2, UserId = 20, Amount = 20.0m };
        var payment3 = new Payment { PaymentId = 3, UserId = 10, Amount = 30.0m };
        context.Payments.AddRange(payment1, payment2, payment3);
        await context.SaveChangesAsync();

        var service = new PaymentService(context);

        // Act
        var result = await service.GetUserPaymentsAsync(10);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.PaymentId == 1 && p.Amount == 15.5m);
        result.Should().Contain(p => p.PaymentId == 3 && p.Amount == 30.0m);
        result.Should().NotContain(p => p.PaymentId == 2);
    }

    [Fact]
    public async Task GetUserPaymentsAsync_UserHasNoPayments_ReturnsEmptyList()
    {
        // Arrange
        using var context = CreateDbContext();
        context.Payments.RemoveRange(context.Payments);

        var payment = new Payment { PaymentId = 1, UserId = 20, Amount = 20.0m };
        context.Payments.Add(payment);
        await context.SaveChangesAsync();

        var service = new PaymentService(context);

        // Act
        var result = await service.GetUserPaymentsAsync(10);

        // Assert
        result.Should().BeEmpty();
    }
}
