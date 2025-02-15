using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Cinema_System.Services.Interfaces;
using Ardalis.Specification;

namespace Cinema_System.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            return Task.FromResult(_unitOfWork.Users.GetById(id)); 
        }

        public Task<List<Ticket>> GetUserPurchasesAsync(int id)
        {
            var tickets = _unitOfWork.Tickets.GetListBySpec(new TicketsByUserSpecification(id));

            return Task.FromResult(tickets.ToList());
        }
    }

    public class TicketsByUserSpecification : Specification<Ticket>
    {
        public TicketsByUserSpecification(int userId)
        {
            Query.Where(t => t.UserId == userId);
        }
    }
}