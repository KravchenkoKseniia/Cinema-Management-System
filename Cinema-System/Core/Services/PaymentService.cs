using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Infrastructure.Entities;
using Cinema_System.Services.Interfaces;

namespace Cinema_System.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<List<Payment>> GetUserPaymentsAsync(int userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
    }
}