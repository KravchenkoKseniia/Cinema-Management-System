using Cinema_System.DTOs;
using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;

namespace MVC.ControllersUI;

public class AuthorizationControllerUI : Controller
{
    private readonly IAuthenticationService _authService;

    public AuthorizationControllerUI(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
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
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
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
