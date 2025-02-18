using AutoMapper;
using Cinema_System.DTOs;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Entities.Specifications;
using Cinema_System.Services.Interfaces;

namespace Cinema_System.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public IEnumerable<TicketDTO> GetTicketsByUser(int userId)
        {
            var specification = new TicketsByUserSpecification(userId);
            var tickets = _unitOfWork.Tickets.GetListBySpec(specification);
            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }
        
        public TicketDTO? GetTicketById(int ticketId)
        {
            var ticket = _unitOfWork.Tickets.GetById(ticketId);
            return ticket == null ? null : _mapper.Map<TicketDTO>(ticket);
        }
        
        public void BookTicket(TicketDTO ticketDto)
        {
            var ticket = _mapper.Map<Ticket>(ticketDto);
            _unitOfWork.Tickets.Insert(ticket);
            _unitOfWork.Save();
        }
    }
}