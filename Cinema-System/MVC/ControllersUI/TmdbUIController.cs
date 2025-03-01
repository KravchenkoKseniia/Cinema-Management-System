using Cinema_System.Services;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MVC.ControllersUI;


public class TmdbUIController : Controller
{
    private readonly ITmdbService _tmdbService;

    public TmdbUIController(ITmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    [HttpGet("now-playing")]
    public async Task<IActionResult> NowPlaying()
    {
        var movies = await _tmdbService.GetNowPlayingMoviesAsync();
        return View(movies);
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> Upcoming()
    {
        var movies = await _tmdbService.GetUpcomingMoviesAsync();
        return View(movies);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string title)
    {
        var movies = await _tmdbService.SearchMoviesByTitleAsync(title);
        return View(movies);
    }

    [HttpGet("details/{movieId}")]
    public async Task<IActionResult> Details(int movieId)
    {
        var movie = await _tmdbService.FetchMovieFromTmdbByIdAsync(movieId);
        if (movie == null) return NotFound();
        return View(movie);
    }
}
