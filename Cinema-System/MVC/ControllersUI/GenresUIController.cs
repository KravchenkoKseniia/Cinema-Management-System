using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace MVC.ControllersUI;

public class GenresUIController : Controller
{
    /*private readonly ApplicationDbContext _context;
    
    public GenresUIController(ApplicationDbContext context)
    {
        _context = context;
    }*/
    
    /*[HttpGet]
    public async Task<IActionResult> GetGenres()
    {
        var genres = await _context.Genres.ToListAsync();
        return View(genres);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetGenresById(int id)
    {
        var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);
        return View(genre);
    }
    
    [HttpGet]
    public IActionResult CreateGenre()
    {
        return View();
    }*/
}