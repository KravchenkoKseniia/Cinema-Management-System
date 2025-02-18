using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;
using Cinema_System.DTOs;
namespace MVC.Controllers;

[Route("api/tickets")]
[ApiController]
public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;
    
    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }
    
    [HttpGet("user/{userId}")]
    public IActionResult GetTicketsByUser(int userId)
    {
        return Ok(_ticketService.GetTicketsByUser(userId));
    }
    
    [HttpPost]
    public IActionResult BookTicket([FromBody] TicketDTO ticketDto)
    {
        _ticketService.BookTicket(ticketDto);
        return Ok(new { message = "Ticket booked successfully" });
    }
}
