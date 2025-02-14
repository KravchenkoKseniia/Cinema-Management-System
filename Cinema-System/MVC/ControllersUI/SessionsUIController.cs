using Microsoft.AspNetCore.Mvc;
using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
namespace MVC.ControllersUI;

public class SessionsUIController : Controller
{
    private readonly ISessionService _sessionService;
    
    public SessionsUIController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    public IActionResult Index()
    {
        var sessions =  _sessionService.GetAllSessions();
        return View(sessions); //Returns Views/Sessions/Index.cshtml
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