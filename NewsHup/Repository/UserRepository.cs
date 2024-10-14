using NewsHup.Models;
using System.Security.Claims;
using TestMVC.Models;

namespace NewsHup.Repository
{
    public class UserRepository : IUserRepository
    {
        //private NewsContext newsContext;
        private readonly NewsContext _newsContext;

        // Use Dependency Injection to inject NewsContext
        public UserRepository(NewsContext newsContext)
        {
            _newsContext = newsContext;
        }
        public List<User> GetAll()
        {
            List<User> users = _newsContext.Users.ToList();
            return users;
        }

        public User GetUserBy(Func<User, bool> GetBy)
        {
            User user = _newsContext.Users.SingleOrDefault(GetBy);
            return user;
        }

        public void Add(User user)
        {
            _newsContext.Users.Add(user);
        }


        public void SaveChanges()
        {
            _newsContext.SaveChanges();
        }

        public int GetLoggerId(HttpContext Req)
        {
            return Convert.ToInt32(Req.User.FindFirstValue("Id"));

        }
    }
}