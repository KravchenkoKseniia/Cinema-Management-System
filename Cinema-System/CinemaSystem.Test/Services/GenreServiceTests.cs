using System;
using System.Threading.Tasks;
using Cinema_System.Services;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CinemaSystem.Test.Services
{
    public class GenreServiceTests
    {
        private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetGenresAsync_ShouldReturnAllGenres()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Genres.RemoveRange(context.Genres);
                await context.SaveChangesAsync();

                context.Genres.Add(new Genre { GenreId = 1, Id = 1, GenreName = "Action" });
                context.Genres.Add(new Genre { GenreId = 2, Id = 2, GenreName = "Comedy" });
                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new ApplicationDbContext(options))
            {
                var service = new GenreService(context);
                var genres = await service.GetGenresAsync();

                Assert.Equal(2, genres.Count);
                Assert.Contains(genres, g => g.GenreName == "Action");
                Assert.Contains(genres, g => g.GenreName == "Comedy");
            }
        }

        [Fact]
        public async Task GetGenreByIdAsync_ShouldReturnGenre_WhenExists()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new ApplicationDbContext(options))
            {
                context.Genres.RemoveRange(context.Genres);
                await context.SaveChangesAsync();

                context.Genres.Add(new Genre { GenreId = 5, Id = 5, GenreName = "Sci-Fi" });
                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new ApplicationDbContext(options))
            {
                var service = new GenreService(context);
                var genre = await service.GetGenreByIdAsync(5);

                Assert.NotNull(genre);
                Assert.Equal("Sci-Fi", genre.GenreName);
            }
        }

        [Fact]
        public async Task GetGenreByIdAsync_ShouldReturnNull_WhenDoesNotExist()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Act & Assert
            using (var context = new ApplicationDbContext(options))
            {
                var service = new GenreService(context);
                var genre = await service.GetGenreByIdAsync(999);

                Assert.Null(genre);
            }
        }

        [Fact]
        public async Task AddGenreAsync_ShouldAddGenreToDatabase()
        {
            // Arrange
            var options = CreateNewContextOptions();
            var newGenre = new Genre { GenreId = 10, Id = 10, GenreName = "Drama" };

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var service = new GenreService(context);
                await service.AddGenreAsync(newGenre);
            }

            // Assert
            using (var context = new ApplicationDbContext(options))
            {
                var genre = await context.Genres.FindAsync(10);
                Assert.NotNull(genre);
                Assert.Equal("Drama", genre.GenreName);
            }
        }
    }
}
