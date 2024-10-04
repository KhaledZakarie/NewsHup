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
        public void Delete(Article article)
        {
            newsContext.Remove(article);
            SaveToDb();
        }

        public List<Article> GetAll()
        {
            List<Article> articles = newsContext.Articles.ToList();
            return articles;
        }
        public Article GetArticleBy(Func<Article, bool> GetBy)
        {
            Article article = newsContext.Articles.FirstOrDefault(GetBy);
            return article;
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
