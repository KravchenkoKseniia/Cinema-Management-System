using Cinema_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

[Route("api/tmdb")]
[ApiController]
public class TmdbController : ControllerBase
{
    private readonly TmdbService _tmdbService;

    public TmdbController(TmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchMovies([FromQuery] string title)
    {
        var result = await _tmdbService.SearchMoviesByTitleAsync(title);
        if (result == null) return NotFound("No movies found.");
        return Ok(result);
    }

    [HttpGet("now-playing")]
    public async Task<IActionResult> GetNowPlayingMovies()
    {
        var result = await _tmdbService.GetNowPlayingMoviesAsync();
        return Ok(result);
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingMovies()
    {
        var result = await _tmdbService.GetUpcomingMoviesAsync();
        return Ok(result);
    }

    [HttpGet("{movieId}")]
    public async Task<IActionResult> GetMovieDetails(int movieId)
    {
        var movie = await _tmdbService.FetchMovieFromTmdbByIdAsync(movieId);
        if (movie == null) return NotFound("Movie not found.");
        return Ok(movie);
    }
}
