using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class SeedDataExtensions
    {
        public static void SeedGenres(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new Genre { GenreId = 1, GenreName = "Action" },
                new Genre { GenreId = 2, GenreName = "Comedy" },
                new Genre { GenreId = 3, GenreName = "Drama" },
                new Genre { GenreId = 4, GenreName = "Horror" },
                new Genre { GenreId = 5, GenreName = "Sci-Fi" },
                new Genre { GenreId = 6, GenreName = "Thriller" },
                new Genre { GenreId = 7, GenreName = "Western" },
                new Genre { GenreId = 8, GenreName = "Animation" },
                new Genre { GenreId = 9, GenreName = "Romance" },
                new Genre { GenreId = 10, GenreName = "Documentary" }
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

        public static void SeedMovies(this ModelBuilder modelBuilder) // TODO: replace fake links with real ones from TMDB
        {
            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    MovieId = 1,
                    Title = "Skyfall",
                    Description = "James Bond faces a new threat.",
                    TrailerURL = "https://trailers.com/skyfall",
                    ReleaseDate = new DateTime(2012, 10, 26),
                    GenreId = 1,
                    Rating = 7.8m,
                    Duration = new TimeSpan(2, 23, 0),
                    PosterURL = "https://posters.com/skyfall.jpg"
                },
                new Movie
                {
                    MovieId = 2,
                    Title = "The Hangover",
                    Description = "A bachelor party goes wrong.",
                    TrailerURL = "https://trailers.com/hangover",
                    ReleaseDate = new DateTime(2009, 6, 5),
                    GenreId = 2,
                    Rating = 7.7m,
                    Duration = new TimeSpan(1, 40, 0),
                    PosterURL = "https://posters.com/hangover.jpg"
                },
                new Movie
                {
                    MovieId = 3,
                    Title = "The Godfather",
                    Description = "The aging patriarch transfers control to his son.",
                    TrailerURL = "https://trailers.com/godfather",
                    ReleaseDate = new DateTime(1972, 3, 24),
                    GenreId = 3,
                    Rating = 9.2m,
                    Duration = new TimeSpan(2, 55, 0),
                    PosterURL = "https://posters.com/godfather.jpg"
                },
                new Movie
                {
                    MovieId = 4,
                    Title = "The Conjuring",
                    Description = "Paranormal investigators help a family.",
                    TrailerURL = "https://trailers.com/conjuring",
                    ReleaseDate = new DateTime(2013, 7, 19),
                    GenreId = 4,
                    Rating = 7.5m,
                    Duration = new TimeSpan(1, 52, 0),
                    PosterURL = "https://posters.com/conjuring.jpg"
                },
                new Movie
                {
                    MovieId = 5,
                    Title = "Inception",
                    Description = "A thief enters people's dreams.",
                    TrailerURL = "https://trailers.com/inception",
                    ReleaseDate = new DateTime(2010, 7, 16),
                    GenreId = 5,
                    Rating = 8.8m,
                    Duration = new TimeSpan(2, 28, 0),
                    PosterURL = "https://posters.com/inception.jpg"
                },
                new Movie
                {
                    MovieId = 6,
                    Title = "Interstellar",
                    Description = "Explorers travel through a wormhole.",
                    TrailerURL = "https://trailers.com/interstellar",
                    ReleaseDate = new DateTime(2014, 11, 7),
                    GenreId = 5,
                    Rating = 8.6m,
                    Duration = new TimeSpan(2, 49, 0),
                    PosterURL = "https://posters.com/interstellar.jpg"
                },
                new Movie
                {
                    MovieId = 7,
                    Title = "John Wick",
                    Description = "Ex-hitman seeks vengeance.",
                    TrailerURL = "https://trailers.com/johnwick",
                    ReleaseDate = new DateTime(2014, 10, 24),
                    GenreId = 1,
                    Rating = 7.4m,
                    Duration = new TimeSpan(1, 41, 0),
                    PosterURL = "https://posters.com/johnwick.jpg"
                },
                new Movie
                {
                    MovieId = 8,
                    Title = "Step Brothers",
                    Description = "Two grown men become stepbrothers.",
                    TrailerURL = "https://trailers.com/stepbrothers",
                    ReleaseDate = new DateTime(2008, 7, 25),
                    GenreId = 2,
                    Rating = 6.9m,
                    Duration = new TimeSpan(1, 38, 0),
                    PosterURL = "https://posters.com/stepbrothers.jpg"
                },
                new Movie
                {
                    MovieId = 9,
                    Title = "Schindler's List",
                    Description = "Story of Oskar Schindler.",
                    TrailerURL = "https://trailers.com/schindlerslist",
                    ReleaseDate = new DateTime(1993, 12, 15),
                    GenreId = 3,
                    Rating = 9.0m,
                    Duration = new TimeSpan(3, 15, 0),
                    PosterURL = "https://posters.com/schindlerslist.jpg"
                },
                new Movie
                {
                    MovieId = 10,
                    Title = "It",
                    Description = "Children face a terrifying clown.",
                    TrailerURL = "https://trailers.com/it",
                    ReleaseDate = new DateTime(2017, 9, 8),
                    GenreId = 4,
                    Rating = 7.3m,
                    Duration = new TimeSpan(2, 15, 0),
                    PosterURL = "https://posters.com/it.jpg"
                },
                new Movie
                {
                    MovieId = 11,
                    Title = "The Dark Knight",
                    Description = "Batman faces the Joker.",
                    TrailerURL = "https://trailers.com/thedarkknight",
                    ReleaseDate = new DateTime(2008, 7, 18),
                    GenreId = 1,
                    Rating = 9.0m,
                    Duration = new TimeSpan(2, 32, 0),
                    PosterURL = "https://posters.com/thedarkknight.jpg"
                },
                new Movie
                {
                    MovieId = 12,
                    Title = "The Avengers",
                    Description = "Superheroes team up to save the world.",
                    TrailerURL = "https://trailers.com/avengers",
                    ReleaseDate = new DateTime(2012, 5, 4),
                    GenreId = 1,
                    Rating = 8.0m,
                    Duration = new TimeSpan(2, 23, 0),
                    PosterURL = "https://posters.com/avengers.jpg"
                },
                new Movie
                {
                    MovieId = 13,
                    Title = "Avatar",
                    Description = "A marine on an alien planet.",
                    TrailerURL = "https://trailers.com/avatar",
                    ReleaseDate = new DateTime(2009, 12, 18),
                    GenreId = 5,
                    Rating = 7.8m,
                    Duration = new TimeSpan(2, 42, 0),
                    PosterURL = "https://posters.com/avatar.jpg"
                },
                new Movie
                {
                    MovieId = 14,
                    Title = "Titanic",
                    Description = "A love story on the ill-fated ship.",
                    TrailerURL = "https://trailers.com/titanic",
                    ReleaseDate = new DateTime(1997, 12, 19),
                    GenreId = 9,
                    Rating = 7.9m,
                    Duration = new TimeSpan(3, 15, 0),
                    PosterURL = "https://posters.com/titanic.jpg"
                },
                new Movie
                {
                    MovieId = 15,
                    Title = "Finding Nemo",
                    Description = "A clownfish searches for his son.",
                    TrailerURL = "https://trailers.com/findingnemo",
                    ReleaseDate = new DateTime(2003, 5, 30),
                    GenreId = 8,
                    Rating = 8.1m,
                    Duration = new TimeSpan(1, 40, 0),
                    PosterURL = "https://posters.com/findingnemo.jpg"
                },
                new Movie
                {
                    MovieId = 16,
                    Title = "The Lion King",
                    Description = "A lion cub's journey to be king.",
                    TrailerURL = "https://trailers.com/lionking",
                    ReleaseDate = new DateTime(1994, 6, 24),
                    GenreId = 8,
                    Rating = 8.5m,
                    Duration = new TimeSpan(1, 28, 0),
                    PosterURL = "https://posters.com/lionking.jpg"
                },
                new Movie
                {
                    MovieId = 17,
                    Title = "Pulp Fiction",
                    Description = "The lives of criminals intertwine.",
                    TrailerURL = "https://trailers.com/pulpfiction",
                    ReleaseDate = new DateTime(1994, 10, 14),
                    GenreId = 6,
                    Rating = 8.9m,
                    Duration = new TimeSpan(2, 34, 0),
                    PosterURL = "https://posters.com/pulpfiction.jpg"
                },
                new Movie
                {
                    MovieId = 18,
                    Title = "The Social Network",
                    Description = "The story of Facebook's creation.",
                    TrailerURL = "https://trailers.com/socialnetwork",
                    ReleaseDate = new DateTime(2010, 10, 1),
                    GenreId = 10,
                    Rating = 7.7m,
                    Duration = new TimeSpan(2, 0, 0),
                    PosterURL = "https://posters.com/socialnetwork.jpg"
                },
                new Movie
                {
                    MovieId = 19,
                    Title = "Joker",
                    Description = "The origin story of the Joker.",
                    TrailerURL = "https://trailers.com/joker",
                    ReleaseDate = new DateTime(2019, 10, 4),
                    GenreId = 6,
                    Rating = 8.4m,
                    Duration = new TimeSpan(2, 2, 0),
                    PosterURL = "https://posters.com/joker.jpg"
                },
                new Movie
                {
                    MovieId = 20,
                    Title = "La La Land",
                    Description = "A musician and actress fall in love.",
                    TrailerURL = "https://trailers.com/lalaland",
                    ReleaseDate = new DateTime(2016, 12, 9),
                    GenreId = 9,
                    Rating = 8.0m,
                    Duration = new TimeSpan(2, 8, 0),
                    PosterURL = "https://posters.com/lalaland.jpg"
                }
            );
        }

        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "User" }
            );
        } // TODO: replace SeedRoles with Identity
    }
} // sessions, tickets, payments, users will be added dynamically
