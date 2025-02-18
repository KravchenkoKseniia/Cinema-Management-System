using Cinema_System.Services.Interfaces;
using Cinema_System.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MVC.ControllersUI;

public class MovieUIController : Controller
{
    private readonly IMovieService _movieService;
    private readonly ITmdbService _tmdbService;
    
    public MovieUIController(IMovieService movieService, ITmdbService tmdbService)
    {
        _movieService = movieService;
        _tmdbService = tmdbService;
    }
    
    [HttpGet("tmdb-search")]
    public IActionResult TmdbSearch()
    {
        return View();
    }

    [HttpPost("tmdb-search")]
    public async Task<IActionResult> TmdbSearchResults(string title)
    {
        var searchResults = await _tmdbService.SearchMoviesByTitleAsync(title);
        return View("TmdbSearchResults", searchResults);
    }
    
    [HttpPost("import/{movieId}")]
    public async Task<IActionResult> ImportFromTmdb(int movieId)
    {
        var movieDto = await _tmdbService.FetchMovieFromTmdbByIdAsync(movieId);

        if (movieDto == null)
        {
            TempData["Message"] = "Movie not found on TMDB.";
            return RedirectToAction(nameof(Index));
        }

        var existingMovie = _movieService.GetMovieById(movieDto.MovieId);
        if (existingMovie != null)
        {
            TempData["Message"] = $"Movie '{movieDto.Title}' already exists.";
            return RedirectToAction(nameof(Index));
        }

        _movieService.CreateMovie(movieDto);

        return RedirectToAction(nameof(Index));
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
