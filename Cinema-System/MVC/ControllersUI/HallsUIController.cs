using Cinema_System.Services;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace MVC.ControllersUI;

public class HallsUIController : Controller
{
    private readonly IHallService _hallService;
    
    public HallsUIController(IHallService hallService)
    {
        _hallService = hallService;
    }
    
    public async Task<IActionResult> Index()
    {
        var halls = await _hallService.GetAllHallsAsync();
        return View(halls); //Returns Views/Halls/Index.cshtml
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var hall = await _hallService.GetHallByIdAsync(id);
        if (hall == null)
        {
            return NotFound();
        }
        
        return View(hall); //Returns Views/Halls/Details.cshtml
    }
}