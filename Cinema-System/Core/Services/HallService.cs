using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Cinema_System.Services.Interfaces;
namespace Cinema_System.Services;

public class HallService : IHallService
{
    private readonly IUnitOfWork _unitOfWork;

    public HallService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Hall>> GetAllHallsAsync()
    {
        return await Task.FromResult(_unitOfWork.Halls.GetAll());
    }

    public async Task<Hall?> GetHallByIdAsync(int id)
    {
        return await Task.FromResult(_unitOfWork.Halls.GetById(id));
    }
}
