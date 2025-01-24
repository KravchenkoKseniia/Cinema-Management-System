namespace CMS.Core.Entities;

public class Hall
{
    public int HallId { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }

    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}
