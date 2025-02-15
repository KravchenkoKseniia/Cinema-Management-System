using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;
using Infrastructure.Entities;
namespace MVC.ControllersUI;

public class GenresUIController : Controller
{
    private readonly IGenreService _genreService;
    
    public GenresUIController(IGenreService genreService)
    {
        _genreService = genreService;
    }
    
    public async Task<IActionResult> Index()
    {
        var genres = await _genreService.GetGenresAsync();
        return View(genres); //Returns Views/Genres/Index.cshtml
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var genre = await _genreService.GetGenreByIdAsync(id);
        if (genre == null)
        {
            return NotFound();
        }
        
        return View(genre); //Returns Views/Genres/Details.cshtml
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View(); //Returns Views/Genres/Create.cshtml
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Genre genre)
    {
        if (ModelState.IsValid)
        {
            await _genreService.AddGenreAsync(genre);
            return RedirectToAction(nameof(Index));
        }
        
        return View(genre); 
    }
}