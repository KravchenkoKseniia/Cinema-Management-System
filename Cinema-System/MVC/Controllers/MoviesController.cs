using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

[Route("api/movies")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public IActionResult GetAllMovies()
    {
        var movies = _movieService.GetAllMovies();
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public IActionResult GetMovieById(int id)
    {
        var movie = _movieService.GetMovieById(id);
        if (movie == null)
            return NotFound("Movie not found");
        
        return Ok(movie);
    }

    [HttpPost]
    public IActionResult CreateMovie([FromBody] MovieDTO movieDto)
    {
        if (movieDto == null)
            return BadRequest("Invalid data");

        _movieService.CreateMovie(movieDto);
        return CreatedAtAction(nameof(GetMovieById), new { id = movieDto.MovieId }, movieDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] MovieDTO movieDto)
    {
        if (movieDto == null || movieDto.MovieId != id)
            return BadRequest("Invalid data");

        _movieService.UpdateMovie(movieDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(int id)
    {
        _movieService.DeleteMovie(id);
        return NoContent();
    }
}
