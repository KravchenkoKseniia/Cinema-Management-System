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
public class RoleServiceTests
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
    public async Task GetRolesAsync_RolesExist_ReturnsAllRoles()
    {
        // Arrange
        using var context = CreateDbContext();
        context.Roles.RemoveRange(context.Roles);
        await context.SaveChangesAsync();

        var role1 = new Role { Id = 1, Name = "Admin", NormalizedName = "ADMIN" };
        var role2 = new Role { Id = 2, Name = "User", NormalizedName = "USER" };
        context.Roles.AddRange(role1, role2);
        await context.SaveChangesAsync();

        var service = new RoleService(context);

        // Act
        var result = await service.GetRolesAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.Id == 1 && r.Name == "Admin");
        result.Should().Contain(r => r.Id == 2 && r.Name == "User");
    }

    [Fact]
    public async Task GetRolesAsync_NoRolesExist_ReturnsEmptyList()
    {
        // Arrange
        using var context = CreateDbContext();
        context.Roles.RemoveRange(context.Roles);
        await context.SaveChangesAsync();

        var service = new RoleService(context);

        // Act
        var result = await service.GetRolesAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
