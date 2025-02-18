namespace Cinema_System.DTOs;

public class TicketDTO
{
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public int SessionId { get; set; }
    public DateTime SessionDate { get; set; }
    public TimeSpan SessionStartTime { get; set; }
    public int HallId { get; set; }
    public string HallName { get; set; } = null!;
    public int SeatRow { get; set; }
    public decimal TicketPrice { get; set; }
    public int PaymentId { get; set; }
    public decimal PaymentAmount { get; set; }
}