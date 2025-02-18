using Microsoft.AspNetCore.Mvc;
using Infrastructure.Interfaces;
using Cinema_System.Services;

namespace MVC.Controllers;

[Route("api/halls")]
[ApiController]
public class HallsController : ControllerBase
{
    private readonly HallService _hallService;

    public HallsController(HallService hallService)
    {
        _hallService = hallService;
    }

    [HttpGet]
    public async Task<IActionResult> GetHalls()
    {
        var halls = _hallService.GetAllHalls();
        return Ok(halls);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHallById(int id)
    {
        var hall = _hallService.GetHallById(id);
        if (hall == null)
        {
            return NotFound();
        }
        return Ok(hall);
    }
}
