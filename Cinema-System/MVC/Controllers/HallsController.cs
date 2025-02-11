using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace MVC.Controllers;

[Route("api/halls")]
[ApiController]
public class HallsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HallsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetHalls()
    {
        var halls = await _context.Halls.ToListAsync();
        return Ok(halls);
    }
}
