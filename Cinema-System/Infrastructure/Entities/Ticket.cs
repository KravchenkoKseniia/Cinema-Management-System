using Infrastructure.Interfaces;

namespace Infrastructure.Entities;

public class Ticket : IBaseEntity
{
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public int SeatRow { get; set; }
    public int PaymentId { get; set; }

    public User User { get; set; } = null!;
    public Session Session { get; set; } = null!;
    public Payment Payment { get; set; } = null!;
}
