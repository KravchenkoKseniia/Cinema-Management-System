namespace CMS.Infrastructure.Entities;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }

    public Role Role { get; set; } = null!;
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
