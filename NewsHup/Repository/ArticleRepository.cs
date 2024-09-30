using NewsHup.Models;
using TestMVC.Models;

namespace NewsHup.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private NewsContext newsContext;

        public ArticleRepository()
        {
            newsContext = new NewsContext();
        }

        public void AddArticle(Article article)
        {
            newsContext.Add(article);
            SaveToDb();
        }

        public List<Article> GetAll()
        {
            List<Article> articles = newsContext.Articles.ToList();
            return articles;
        }

        public List<Article> GetArticlesBy(Func<Article, bool> GetBy)
        {
            List<Article> articles = newsContext.Articles.Where(GetBy).ToList();
            return articles;
        }

        public void SaveToDb()
        {
            newsContext.SaveChanges();
        }
    }
}
