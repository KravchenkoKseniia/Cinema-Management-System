using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class UnitOfWork: IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    private IGenericRepository<User>? _userRepository;
    private IGenericRepository<Role>? _roleRepository;
    private IGenericRepository<Ticket>? _ticketRepository;
    private IGenericRepository<Session>? _sessionRepository;
    private IGenericRepository<Hall>? _hallRepository;
    private IGenericRepository<Payment>? _paymentRepository;
    private IGenericRepository<Genre>? _genreRepository;
    private IGenericRepository<Movie>? _movieRepository;
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IGenericRepository<User> Users
        => _userRepository ??= new GenericRepository<User>(_context);
    
    public IGenericRepository<Role> Roles
        => _roleRepository ??= new GenericRepository<Role>(_context);
    
    public IGenericRepository<Ticket> Tickets
        => _ticketRepository ??= new GenericRepository<Ticket>(_context);
    
    public IGenericRepository<Session> Sessions
        => _sessionRepository ??= new GenericRepository<Session>(_context);
    
    public IGenericRepository<Hall> Halls
        => _hallRepository ??= new GenericRepository<Hall>(_context);
    
    public IGenericRepository<Payment> Payments 
        => _paymentRepository ??= new GenericRepository<Payment>(_context);
    
    public IGenericRepository<Genre> Genres
        => _genreRepository ??= new GenericRepository<Genre>(_context);
    
    public IGenericRepository<Movie> Movies
        => _movieRepository ??= new GenericRepository<Movie>(_context);

    public int Save()
    {
       return _context.SaveChanges();
    }

    #region IDisposable
        
    private bool _disposed = false;
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    #endregion
}