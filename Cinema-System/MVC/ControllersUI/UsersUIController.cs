using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;
namespace MVC.ControllersUI;

public class UsersUIController : Controller
{
    private readonly IUserService _userService;
    
    public UsersUIController(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();
        return View(user); //Returns Views/Users/Details.cshtml
    }
    
    public async Task<IActionResult> Purchases(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();
        var purchases = await _userService.GetUserPurchasesAsync(id);
        return View(purchases); //Returns Views/Users/Purchases.cshtml
    }
}