using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace MVC.ControllersUI;

public class HallsUIController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public HallsUIController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetHalls()
    {
        var halls = await _context.Halls.ToListAsync();
        return View(halls);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetHallsById(int id)
    {
        var hall = await _context.Halls.FirstOrDefaultAsync(x => x.Id == id);
        return View(hall);
    }
    
    [HttpGet]
    public IActionResult CreateHall()
    {
        return View();
    }   
}