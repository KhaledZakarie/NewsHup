using NewsHup.Models;

namespace NewsHup.Repository
{
    public interface IUserRepository
    {
        public User GetUserBy(Func<User, bool> GetBy);
    }
}
