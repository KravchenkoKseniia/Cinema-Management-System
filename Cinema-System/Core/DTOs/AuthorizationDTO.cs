namespace Cinema_System.DTOs;

public class RegisterDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginDto
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}