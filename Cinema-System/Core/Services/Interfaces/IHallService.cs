using Infrastructure.Entities;

namespace Cinema_System.Services.Interfaces;

public interface IHallService
{
    Task<IEnumerable<Hall>> GetAllHallsAsync();
    Task<Hall?> GetHallByIdAsync(int id);
}
