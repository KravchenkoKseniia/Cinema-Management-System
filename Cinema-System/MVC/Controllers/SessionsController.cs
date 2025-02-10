using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace MVC.Controllers;

[Route("api/sessions")]
[ApiController]
public class SessionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SessionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetSessions()
    {
        var sessions = await _context.Sessions.Include(s => s.Movie).ToListAsync();
        return Ok(sessions);
    }
}
