using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface ISessionRepo
    {
        public Sessions? GetSessionByUserId(string UserId);
        public void CreateSession(Sessions CSession);
        public void EditSession(string UserId, Sessions ESession);
        public void DeleteSessions(string UserId);
    }
}
