using Cinema_System.DTOs;
using System.Collections.Generic;

namespace Cinema_System.Services.Interfaces
{
    public interface ITicketService
    {
        IEnumerable<TicketDTO> GetTicketsByUser(int userId);
        TicketDTO? GetTicketById(int ticketId);
        void BookTicket(TicketDTO ticketDto);
    }
}