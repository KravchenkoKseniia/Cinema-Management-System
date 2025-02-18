using AutoMapper;
using Cinema_System.DTOs;
using Infrastructure.Entities;
using Infrastructure.Entities.Specifications;
using Infrastructure.Interfaces;
using Cinema_System.Services.Interfaces;

namespace Cinema_System.Services
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public IEnumerable<SessionDTO> GetAllSessions()
        {
            var specification = new AllSessionsSpecification();
            var sessions = _unitOfWork.Sessions.GetListBySpec(specification);

            return _mapper.Map<IEnumerable<SessionDTO>>(sessions);
        }

        public IEnumerable<SessionDTO> GetSessionsByMovieAndDate(int movieId, DateTime date)
        {
            var specification = new SessionsByMovieAndDataSpecification(movieId, date);
            var sessions = _unitOfWork.Sessions.GetListBySpec(specification);
            return _mapper.Map<IEnumerable<SessionDTO>>(sessions);
        }
        
        public IEnumerable<SessionDTO> GetSessionsByMovie(int movieId)
        {
            var specification = new SessionsByMovieSpecification(movieId);
            var sessions = _unitOfWork.Sessions.GetListBySpec(specification);
            return _mapper.Map<IEnumerable<SessionDTO>>(sessions);
        }

        public SessionDTO? GetSessionById(int sessionId)
        {
            var session = _unitOfWork.Sessions.GetById(sessionId);
            return session is null ? null : _mapper.Map<SessionDTO>(session);
        }

        public void CreateSession(SessionDTO sessionDto)
        {
            var movie = _unitOfWork.Movies.GetAll().FirstOrDefault(m => m.Title == sessionDto.MovieTitle);
            if (movie == null)
            {
                Console.WriteLine("Movie with name " + sessionDto.MovieTitle + " does not exist");
                return;
            }
            
            var hall = _unitOfWork.Halls.GetAll().FirstOrDefault(h => h.Name == sessionDto.HallName);
            if (hall == null)
            {
                Console.WriteLine(sessionDto.HallName + " hall does not exist");
                return;
            }
            
            sessionDto.HallId = hall.HallId;
            sessionDto.MovieId = movie.MovieId;
            
            var session = new Session
            {
                MovieId = sessionDto.MovieId,
                HallId = sessionDto.HallId,
                Date = sessionDto.Date,
                StartTime = sessionDto.StartTime,
                EndTime = sessionDto.EndTime,
                TicketPrice = sessionDto.TicketPrice
            };
            
            _unitOfWork.Sessions.Insert(session);
            _unitOfWork.Save();
        }

        public void UpdateSession(SessionDTO sessionDto)
        {
            var session = _unitOfWork.Sessions.GetById(sessionDto.SessionId);
            if (session is null) return;

            _mapper.Map(sessionDto, session);
            _unitOfWork.Sessions.Update(session);
            _unitOfWork.Save();
        }

        public void DeleteSession(int sessionId)
        {
            _unitOfWork.Sessions.Delete(sessionId);
            _unitOfWork.Save();
        }
    }
}