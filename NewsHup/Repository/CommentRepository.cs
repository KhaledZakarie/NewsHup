using NewsHup.Models;
using TestMVC.Models;

namespace NewsHup.Repository
{
    public class CommentRepository : ICommentRepository
    {
        //private NewsContext newsContext;

        private readonly NewsContext _newsContext;

        // Use Dependency Injection to inject NewsContext
        public CommentRepository(NewsContext newsContext)
        {
            _newsContext = newsContext;
        }

        public List<Comment> GetAll()
        {
            List<Comment> comments = _newsContext.Comments.ToList();
            return comments;
        }
        public List<Comment> GetCommentsBy(Func<Comment, bool> GetBy)
        {
            List<Comment> comments = _newsContext.Comments.Where(GetBy).ToList();
            return comments;
        }
        public void Add(Comment comment)
        {
            _newsContext.Comments.Add(comment);
            SaveToDb();
        }

        public void SaveToDb()
        {
            _newsContext.SaveChanges();
        }
    }
}
