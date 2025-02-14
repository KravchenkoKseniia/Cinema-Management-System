using Cinema_System.Services.Interfaces;
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
        return View(movies);
    }

    [HttpGet("{id}")]
    public IActionResult Details(int id)
    {
        var movie = _movieService.GetMovieById(id);
        if (movie == null)
            return NotFound();
        
        return View(movie);
    }
}
