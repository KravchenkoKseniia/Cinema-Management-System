using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;
using Cinema_System.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace MVC.ControllersUI;

[Authorize]
public class TicketsUIController : Controller
{
    private readonly ITicketService _ticketService;
    
    public TicketsUIController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }
    
    public IActionResult Index()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return BadRequest();
        }
        
        var tickets =  _ticketService.GetTicketsByUser(userId);
        return View(tickets); //Returns Views/Tickets/Index.cshtml
    }
    
    [HttpGet]
    public IActionResult Book()
    {
        return View(); //Returns Views/Tickets/Book.cshtml
    }
    
    [HttpPost]
    public IActionResult Book(TicketDTO ticketDto)
    {
        if (ModelState.IsValid)
        {
            _ticketService.BookTicket(ticketDto);
            return RedirectToAction(nameof(Index));
        }
        
        return View(ticketDto);
    }
}