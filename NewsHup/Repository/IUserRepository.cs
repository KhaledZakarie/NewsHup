using NewsHup.Models;

namespace NewsHup.Repository
{
    public interface IUserRepository
    {
        public User GetUserBy(Func<User, bool> GetBy);
        public void Add(User user);
        public void SaveChanges();
        public List<User> GetAll();
    }
}
