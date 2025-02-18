using Infrastructure.Entities;

namespace Cinema_System.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetPaymentsAsync();
        Task<List<Payment>> GetUserPaymentsAsync(int userId);
    }
}