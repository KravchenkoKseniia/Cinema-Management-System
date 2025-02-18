using Cinema_System.DTOs;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthorizationController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDTO model)
    {
        var result = _authService.Register(model.UserName, model.Password, model.Email, "User");
        if (result != "Registration successful!")
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO model)
    {
        var result = _authService.Login(model.UserName, model.Password);
        if (result != "Login successful!")
            return Unauthorized(result);

        return Ok(result);
    }
}
