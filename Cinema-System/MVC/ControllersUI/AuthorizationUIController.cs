using Cinema_System.DTOs;
using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;

namespace MVC.ControllersUI;

public class AuthorizationUIController : Controller
{
    private readonly IAuthenticationService _authService;

    public AuthorizationUIController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(); //Returns Views/Authorization/Login.cshtml
    }

    [HttpPost]
    public IActionResult Login(LoginDTO model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = _authService.Login(model.UserName, model.Password);
        if (result == "Login successful!")
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError("", "Invalid username or password.");
        return View(model); //Returns Views/Authorization/Login.cshtml
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(); //Returns Views/Authorization/Register.cshtml
    }

    [HttpPost]
    public IActionResult Register(RegisterDTO model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = _authService.Register(model.UserName, model.Password, "User");
        if (result == "Registration successful!")
            return RedirectToAction("Login");

        ModelState.AddModelError("", result);
        return View(model);
    }
}
