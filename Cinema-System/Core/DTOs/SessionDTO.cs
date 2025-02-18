namespace Cinema_System.DTOs;

public class SessionDTO
{
    public int SessionId { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public DateTime Date { get; set; }
    public int HallId { get; set; }
    public string HallName { get; set; } = null!;
    public decimal TicketPrice { get; set; }
}