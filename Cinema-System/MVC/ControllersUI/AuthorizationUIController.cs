using Cinema_System.DTOs;
using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Entities;

namespace MVC.ControllersUI;

public class AuthorizationUIController : Controller
{
    private readonly IAuthenticationService _authService;
    private readonly SignInManager<User> _signInManager;

    public AuthorizationUIController(IAuthenticationService authService, SignInManager<User> signInManager)
    {
        _authService = authService;
        _signInManager = signInManager;
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

        if (model.UserName == "admin" && model.Password == "password")
        {
            return RedirectToAction("Index", "AdminView");
        }
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

        var result = _authService.Register(model.UserName, model.Password, model.Email, "User");
        if (result == "Registration successful!")
            return RedirectToAction("Login");

        ModelState.AddModelError("", result);
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
