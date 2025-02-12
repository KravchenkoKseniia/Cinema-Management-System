using Cinema_System.DTOs;

namespace Cinema_System.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionDTO> GetAllSessions();
        IEnumerable<SessionDTO> GetSessionsByMovieAndDate(int movieId, DateTime date);
        SessionDTO? GetSessionById(int sessionId);
        void CreateSession(SessionDTO sessionDto);
        void UpdateSession(SessionDTO sessionDto);
        void DeleteSession(int sessionId);
    }
}