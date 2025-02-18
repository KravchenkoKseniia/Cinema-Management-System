using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;
namespace MVC.ControllersUI;

public class RolesUIController : Controller
{
    private readonly IRoleService _roleService;
    
    public RolesUIController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    public async Task<IActionResult> Index()
    {
        var roles = await _roleService.GetRolesAsync();
        return View(roles); // Returns Views/Roles/Index.cshtml
    }
}