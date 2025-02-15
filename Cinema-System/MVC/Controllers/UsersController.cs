using Microsoft.AspNetCore.Mvc;
using Cinema_System.Services.Interfaces;
namespace MVC.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("{id}/purchases")]
    public async Task<IActionResult> GetUserPurchases(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var tickets = await _userService.GetUserPurchasesAsync(id);
        return Ok(tickets);
    }
}
