using NewsHup.Models;
using TestMVC.Models;

namespace NewsHup.Repository
{
    public class UserRepository : IUserRepository
    {
        private NewsContext newsContext;

        public UserRepository()
        {
            newsContext = new NewsContext();
        }



        public User GetUserBy(Func<User, bool> GetBy)
        {
            User user = newsContext.Users.SingleOrDefault(GetBy);
            return user;
        }
    }
}
