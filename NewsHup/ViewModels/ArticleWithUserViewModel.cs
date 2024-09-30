using NewsHup.Models;

namespace NewsHup.ViewModels
{
    public class ArticleWithUserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
