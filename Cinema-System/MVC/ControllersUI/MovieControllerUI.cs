using Cinema_System.Services.Interfaces;
using Cinema_System.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MVC.ControllersUI;

[Route("movies")]
public class MovieControllerUI : Controller
{
    private readonly IMovieService _movieService;
    
    public MovieControllerUI(IMovieService movieService)
    {
        _movieService = movieService;
    }
    
    public IActionResult Index()
    {
        var movies = _movieService.GetAllMovies();
        return View(movies); //Returns Views/Movies/Index.cshtml
    }
    
    public IActionResult Details(int id)
    {
        var movie = _movieService.GetMovieById(id);
        if (movie == null)
        {
            return NotFound();
        }
        
        return View(movie); //Returns Views/Movies/Details.cshtml
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View(); //Returns Views/Movies/Create.cshtml
    }
    
    [HttpPost]
    public IActionResult Create(MovieDTO movieDto)
    {
        if (ModelState.IsValid)
        {
            _movieService.CreateMovie(movieDto);
            return RedirectToAction(nameof(Index));
        }
        
        return View(movieDto);
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var movie = _movieService.GetMovieById(id);
        if (movie == null)
        {
            return NotFound();
        }
        
        return View(movie); //Returns Views/Movies/Edit.cshtml
    }
    
    [HttpPost]
    public IActionResult Edit(int id, MovieDTO movieDto)
    {
        if (id != movieDto.MovieId)
        {
            return NotFound();
        }
        
        if (ModelState.IsValid)
        {
            _movieService.UpdateMovie(movieDto);
            return RedirectToAction(nameof(Index));
        }
        
        return View(movieDto); 
    }
    
    [HttpPost]
    public IActionResult Delete(int id)
    {
        _movieService.DeleteMovie(id);
        return RedirectToAction(nameof(Index));
    }
}
