using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;
namespace MVC.Controllers;

[Route("api/payments")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPayments()
    {
        var payments = await _paymentService.GetPaymentsAsync();
        return Ok(payments);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserPayments(int userId)
    {
        var payments = await _paymentService.GetUserPaymentsAsync(userId);
        return Ok(payments);
    }
}
