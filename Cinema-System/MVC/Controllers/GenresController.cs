using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;
using Infrastructure.Entities;

namespace MVC.Controllers;

[Route("api/genres")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGenres()
    {
        var genres = await _genreService.GetGenresAsync();
        return Ok(genres);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenreById(int id)
    {
        var genre = await _genreService.GetGenreByIdAsync(id);
        if (genre == null)
        {
            return NotFound();
        }

        return Ok(genre);
    }

    [HttpPost]
    public async Task<IActionResult> AddGenre([FromBody] Genre genre)
    {
        if (genre == null)
        {
            return BadRequest("Invalid genre data");
        }

        await _genreService.AddGenreAsync(genre);
        return CreatedAtAction(nameof(GetGenreById), new { id = genre.Id }, genre);
    }
}
