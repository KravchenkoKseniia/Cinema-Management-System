using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class SeedDataExtensions
    {
        public static void SeedGenres(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new Genre { GenreId = 28, GenreName = "Action" },
                new Genre { GenreId = 12, GenreName = "Adventure" },
                new Genre { GenreId = 16, GenreName = "Animation" },
                new Genre { GenreId = 35, GenreName = "Comedy" },
                new Genre { GenreId = 80, GenreName = "Crime" },
                new Genre { GenreId = 99, GenreName = "Documentary" },
                new Genre { GenreId = 18, GenreName = "Drama" },
                new Genre { GenreId = 10751, GenreName = "Family" },
                new Genre { GenreId = 14, GenreName = "Fantasy" },
                new Genre { GenreId = 36, GenreName = "History" },
                new Genre { GenreId = 27, GenreName = "Horror" },
                new Genre { GenreId = 10402, GenreName = "Music" },
                new Genre { GenreId = 9648, GenreName = "Mystery" },
                new Genre { GenreId = 10749, GenreName = "Romance" },
                new Genre { GenreId = 878, GenreName = "Science Fiction" },
                new Genre { GenreId = 10770, GenreName = "TV Movie" },
                new Genre { GenreId = 53, GenreName = "Thriller" },
                new Genre { GenreId = 10752, GenreName = "War" },
                new Genre { GenreId = 37, GenreName = "Western" }
            );
        }

        public static void SeedHalls(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hall>().HasData(
                new Hall { HallId = 1, Name = "Hall 1", Capacity = 100 },
                new Hall { HallId = 2, Name = "Hall 2", Capacity = 150 },
                new Hall { HallId = 3, Name = "Hall 3", Capacity = 200 }
            );
        }
    }
}
