using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;
using Cinema_System.DTOs;
namespace MVC.Controllers;

[Route("api/sessions")]
[ApiController]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;
    
    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    [HttpGet]
    public IActionResult GetAllSessions()
    {
        return Ok(_sessionService.GetAllSessions());
    }
    
    [HttpGet("{id}")]
    public IActionResult GetSessionById(int id)
    {
        var session = _sessionService.GetSessionById(id);
        return session != null ? Ok(session) : NotFound();
    }
    
    [HttpPost]
    public IActionResult CreateSession([FromBody] SessionDTO sessionDto)
    {
        _sessionService.CreateSession(sessionDto);
        return CreatedAtAction(nameof(GetSessionById), new { id = sessionDto.SessionId }, sessionDto);
    }
}
