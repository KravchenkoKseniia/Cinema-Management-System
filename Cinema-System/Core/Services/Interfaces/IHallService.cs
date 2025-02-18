using Cinema_System.DTOs;
using Infrastructure.Entities;

namespace Cinema_System.Services.Interfaces;

public interface IHallService
{
    IEnumerable<HallDTO> GetAllHalls();
    HallDTO? GetHallById(int id);
}
