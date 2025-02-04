using Microsoft.AspNetCore.Identity;
namespace Infrastructure.Entities;

public class User : IdentityUser<int>
{
    public int RoleId { get; set; }
    
    public Role Role { get; set; } = null!;
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}