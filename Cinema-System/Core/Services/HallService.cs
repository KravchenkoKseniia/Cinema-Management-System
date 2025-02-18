using AutoMapper;
using Cinema_System.DTOs;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Cinema_System.Services.Interfaces;
namespace Cinema_System.Services;

public class HallService : IHallService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HallService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IEnumerable<HallDTO> GetAllHalls()
    {
        var halls = _unitOfWork.Halls.GetAll();
        return _mapper.Map<IEnumerable<HallDTO>>(halls);
    }


    public HallDTO? GetHallById(int id)
    {
        var hall = _unitOfWork.Halls.GetById(id);
        return hall is null ? null : _mapper.Map<HallDTO>(hall);
    }
}
