using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IUnitOfWork: IDisposable
{
    
    IGenericRepository<Genre> Genres { get; }
    IGenericRepository<Movie> Movies { get; }
    IGenericRepository<Session> Sessions { get; }
    IGenericRepository<Hall> Halls { get; }
    IGenericRepository<Ticket> Tickets { get; }
    IGenericRepository<Payment> Payments { get; }
    IGenericRepository<Role> Roles { get; }
    IGenericRepository<User> Users { get; }
    
    int Save();
    
}