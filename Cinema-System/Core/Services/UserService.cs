using AutoMapper;
using Cinema_System.DTOs;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Entities.Specifications;

namespace Cinema_System.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public IEnumerable<MovieDTO> GetAvailableMovies()
        {
            var movies = _unitOfWork.Movies.GetListBySpec(new MoviesByGenreSpecification(0)); // Assume 0 for all genres
            return _mapper.Map<IEnumerable<MovieDTO>>(movies);
        }
        
        public IEnumerable<SessionDTO> GetSessionsByMovieAndDate(int movieId, DateTime date)
        {
            var sessions = _unitOfWork.Sessions.GetListBySpec(new SessionsByMovieAndDataSpecification(movieId, date));
            return _mapper.Map<IEnumerable<SessionDTO>>(sessions);
        }
        
        public IEnumerable<TicketDTO> GetAvailableSeats(int sessionId)
        {
            var tickets = _unitOfWork.Tickets.GetListBySpec(new AvailableSeatsSpecification(sessionId));
            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }
        
        public MovieDTO? GetMovieDetails(int movieId)
        {
            var movie = _unitOfWork.Movies.GetById(movieId);
            return movie == null ? null : _mapper.Map<MovieDTO>(movie);
        }
        
        //public void BookTicket(int userId, int sessionId, int seatNumber) {}
        
        public IEnumerable<TicketDTO> GetUserTickets(int userId)
        {
            var tickets = _unitOfWork.Tickets.GetListBySpec(new TicketsByUserSpecification(userId));
            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }
        
        public IEnumerable<SessionDTO> GetUpcomingSessions()
        {
            var sessions = _unitOfWork.Sessions.GetListBySpec(new UpcomingSessionsSpecification());
            return _mapper.Map<IEnumerable<SessionDTO>>(sessions);
        }
    }
}
