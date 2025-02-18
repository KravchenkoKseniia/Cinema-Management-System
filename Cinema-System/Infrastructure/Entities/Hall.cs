using Infrastructure.Interfaces;

namespace Infrastructure.Entities;

public class Hall : IBaseEntity
{
    public int HallId { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }

    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}