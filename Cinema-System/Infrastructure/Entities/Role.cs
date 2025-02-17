using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;

public class Role : IdentityRole<int>
{
    public ICollection<User> Users { get; set; } = new List<User>();
}

