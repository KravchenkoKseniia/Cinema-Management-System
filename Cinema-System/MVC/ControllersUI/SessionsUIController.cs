using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
namespace MVC.ControllersUI;

public class SessionsUIController : Controller
{
    private readonly ISessionService _sessionService;
    private readonly IMovieService _movieService;
    private readonly IHallService _hallService;

    public SessionsUIController(ISessionService sessionService, IMovieService movieService, IHallService hallService)
    {
        _sessionService = sessionService;
        _movieService = movieService;
        _hallService = hallService;
    }

    public IActionResult Index(string? movieName, DateTime? date)
    {
        IEnumerable<SessionDTO> sessions;

        if (!string.IsNullOrEmpty(movieName) && date.HasValue)
        {
            var movieId = _movieService.GetMovieIdByName(movieName);

            if (movieId == 0)
            {
                return View(new List<SessionDTO>());
            }

            sessions = _sessionService.GetSessionsByMovieAndDate(movieId, date.Value);
        }
        else if (!string.IsNullOrEmpty(movieName))
        {
            var movieId = _movieService.GetMovieIdByName(movieName);

            if (movieId == 0)
            {
                return View(new List<SessionDTO>());
            }

            sessions = _sessionService.GetSessionsByMovie(movieId);
        }
        else
        {
            sessions = _sessionService.GetAllSessions();
        }

        return View(sessions);
    }

    public IActionResult Details(int id)
    {
        var session = _sessionService.GetSessionById(id);
        if (session == null)
        {
            return NotFound();
        }

        return View(session);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Movies = new SelectList(_movieService.GetAllMovies(), "MovieId", "Title");
        ViewBag.Halls = new SelectList(_hallService.GetAllHalls(), "HallId", "Name");
        return View();
    }

    [HttpPost]
    public IActionResult Create(SessionDTO sessionDto)
    {
        if (ModelState.IsValid)
        {
            _sessionService.CreateSession(sessionDto);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Movies = new SelectList(_movieService.GetAllMovies(), "MovieId", "Title", sessionDto.MovieId);
        ViewBag.Halls = new SelectList(_hallService.GetAllHalls(), "HallId", "Name", sessionDto.HallId);
        return View(sessionDto);
    }
    
    [HttpPost]
    public IActionResult Delete(int id)
    {
        _sessionService.DeleteSession(id);
        return RedirectToAction(nameof(Index));
    }
    
}