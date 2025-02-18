using System.Security.Claims;
using Cinema_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace MVC.ControllersUI;


[Authorize]
public class PaymentsUIController : Controller
{
    
    private readonly IPaymentService _paymentService;
    
    public PaymentsUIController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    
    public async Task<IActionResult> Index()
    {
        var payments = await _paymentService.GetPaymentsAsync();
        return View(payments); // Returns Views/Payments/Index.cshtml
    }
    
    public async Task<IActionResult> Details()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return BadRequest();
        }
        
        var payments = await _paymentService.GetUserPaymentsAsync(userId);
        return View(payments); // Returns Views/Payments/Details.cshtml
    }
}