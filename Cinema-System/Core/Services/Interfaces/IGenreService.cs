using Infrastructure.Entities;

namespace Cinema_System.Services.Interfaces
{
    public interface IGenreService
    {
        Task<List<Genre>> GetGenresAsync();
        Task<Genre?> GetGenreByIdAsync(int id);
        Task AddGenreAsync(Genre genre);
    }
}
