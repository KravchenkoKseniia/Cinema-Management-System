namespace Cinema_System.DTOs;

public class PaymentDTO
{
    public int PaymentId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}