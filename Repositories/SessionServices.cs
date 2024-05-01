using System.Security.Cryptography;
using WebApplication1.Models;

namespace WebApplication1.Repositories

{
    public class SessionServices : ISessionRepo
    {
        public ECommerceContext _Context { get; set; }
        public SessionServices(ECommerceContext context)
        {
            _Context = context;
        }
        public void CreateSession(Sessions CSession)
        {
            if (CSession != null)
            {
                _Context.Sessions.Add(CSession);
                _Context.SaveChanges();
            }
        }

        public void DeleteSessions(string UserId)
        {
            Sessions? tempsession = _Context.Sessions.Where(s => s.ApplicationUserId == UserId).FirstOrDefault();
            if (tempsession != null )
            {
                _Context.Sessions.Remove(tempsession);
                _Context.SaveChanges();

            }
        }

       public void EditSession(string UserId, Sessions ESession)
        {

            Sessions? tempsession = _Context.Sessions.Where(s => s.ApplicationUserId == UserId).FirstOrDefault();
            if (tempsession != null)
            {
                tempsession.LastSessionTime = ESession.LastSessionTime;
                _Context.SaveChanges();

            }
        }

        public Sessions? GetSessionByUserId(string UserId)
        {
            return _Context.Sessions.FirstOrDefault(s => s.ApplicationUserId == UserId);

        }
    }
}
