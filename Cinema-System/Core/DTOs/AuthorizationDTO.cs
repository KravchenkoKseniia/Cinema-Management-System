namespace Cinema_System.DTOs;

public class RegisterDTO
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginDTO
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}