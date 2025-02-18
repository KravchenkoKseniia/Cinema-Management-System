using Infrastructure.Interfaces;

namespace Infrastructure.Entities;

public class Session : IBaseEntity
{
    public int SessionId { get; set; }
    public int MovieId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public DateTime Date { get; set; }
    public int HallId { get; set; }
    public decimal TicketPrice { get; set; }

    public Movie Movie { get; set; } = null!;
    public Hall Hall { get; set; } = null!;
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
