using Cinema_System.DTOs;

namespace Cinema_System.Services.Interfaces
{
    public interface IMovieService
    {
        IEnumerable<MovieDTO> GetAllMovies();
        MovieDTO? GetMovieById(int id);
        void CreateMovie(MovieDTO movieDto);
        void UpdateMovie(MovieDTO movieDto);
        void DeleteMovie(int id);
    }
}