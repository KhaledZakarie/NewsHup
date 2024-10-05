using NewsHup.Models;

namespace NewsHup.Repository
{
    public interface ICommentRepository
    {
        public List<Comment> GetAll();
        public List<Comment> GetCommentsBy(Func<Comment, bool> GetBy);
        public void SaveToDb();
        public void Add(Comment comment);
    }
}
