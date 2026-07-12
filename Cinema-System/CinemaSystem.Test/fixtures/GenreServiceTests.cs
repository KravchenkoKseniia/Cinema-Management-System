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
public class GenreServiceTests
{
    private ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        var context = new ApplicationDbContext(options);
        // Ensure database is created (which runs model configurations, seed data, etc.)
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task GetGenresAsync_GenresExist_ReturnsAllGenres()
    {
        // Arrange
        using var context = CreateDbContext();
        
        // Let's clear any seeded genres to have a controlled test data set
        context.Genres.RemoveRange(context.Genres);
        await context.SaveChangesAsync();

        var genre1 = new Genre { GenreId = 1, GenreName = "Action" };
        var genre2 = new Genre { GenreId = 2, GenreName = "Comedy" };
        context.Genres.AddRange(genre1, genre2);
        await context.SaveChangesAsync();

        var service = new GenreService(context);

        // Act
        var result = await service.GetGenresAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(g => g.GenreId == 1 && g.GenreName == "Action");
        result.Should().Contain(g => g.GenreId == 2 && g.GenreName == "Comedy");
    }

    [Fact]
    public async Task GetGenresAsync_NoGenresExist_ReturnsEmptyList()
    {
        // Arrange
        using var context = CreateDbContext();
        context.Genres.RemoveRange(context.Genres);
        await context.SaveChangesAsync();

        var service = new GenreService(context);

        // Act
        var result = await service.GetGenresAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGenreByIdAsync_GenreExists_ReturnsGenre()
    {
        // Arrange
        using var context = CreateDbContext();
        context.Genres.RemoveRange(context.Genres);
        
        var genre = new Genre { GenreId = 10, GenreName = "Sci-Fi" };
        context.Genres.Add(genre);
        await context.SaveChangesAsync();

        var service = new GenreService(context);

        // Act
        var result = await service.GetGenreByIdAsync(10);

        // Assert
        result.Should().NotBeNull();
        result!.GenreId.Should().Be(10);
        result.GenreName.Should().Be("Sci-Fi");
    }

    [Fact]
    public async Task GetGenreByIdAsync_GenreDoesNotExist_ReturnsNull()
    {
        // Arrange
        using var context = CreateDbContext();
        var service = new GenreService(context);

        // Act
        var result = await service.GetGenreByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddGenreAsync_ValidGenre_AddsGenreToDatabase()
    {
        // Arrange
        using var context = CreateDbContext();
        context.Genres.RemoveRange(context.Genres);
        await context.SaveChangesAsync();

        var service = new GenreService(context);
        var genre = new Genre { GenreId = 5, GenreName = "Drama" };

        // Act
        await service.AddGenreAsync(genre);

        // Assert
        var savedGenre = await context.Genres.FindAsync(5);
        savedGenre.Should().NotBeNull();
        savedGenre!.GenreName.Should().Be("Drama");
    }
}
