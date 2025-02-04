using Infrastructure.Interfaces;

namespace Infrastructure.Entities;

public class Role : IBaseEntity
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();
}
