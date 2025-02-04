using Infrastructure.Interfaces;

namespace Infrastructure.Entities;

public class Payment : IBaseEntity
{
    public int PaymentId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public User User { get; set; } = null!;
}
