using Infrastructure.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Cinema_System.Services.Interfaces;

public interface IUserService
{
    Task<User> GetUserByIdAsync(int id);
    Task<List<Ticket>> GetUserPurchasesAsync(int id);
}
