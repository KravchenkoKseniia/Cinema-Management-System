using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;

public class Role :  IdentityRole<int> 
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();
}
