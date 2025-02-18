using Microsoft.AspNetCore.Mvc;
using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
namespace MVC.ControllersUI;

public class SessionsUIController : Controller
{
    private readonly ISessionService _sessionService;
    private readonly IMovieService _movieService;
    
    public SessionsUIController(ISessionService sessionService, IMovieService movieService)
    {
        _sessionService = sessionService;
        _movieService = movieService;
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
        else if (date.HasValue)
        {
            sessions = _sessionService.GetSessionsByMovieAndDate(0, date.Value);
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
        
        return View(session); //Returns Views/Sessions/Details.cshtml
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View(); //Returns Views/Sessions/Create.cshtml
    }
    
    [HttpPost]
    public IActionResult Create(SessionDTO sessionDto)
    {
        if (ModelState.IsValid)
        {
            _sessionService.CreateSession(sessionDto);
            return RedirectToAction(nameof(Index));
        }
        
        return View(sessionDto);
    }
}