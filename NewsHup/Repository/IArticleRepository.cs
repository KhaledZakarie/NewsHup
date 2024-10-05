using Microsoft.AspNetCore.Mvc;
using NewsHup.Models;

namespace NewsHup.Repository
{
    public interface IArticleRepository
    {
        public List<Article> GetAll();
        //public Article GetArticle(Func<Article, bool> GetBy);
        public List<Article> GetArticlesBy(Func<Article, bool> GetBy);
        public Article GetArticleBy(Func<Article, bool> GetBy);
        public void AddArticle(Article article);
        public void Edit(Article article);
        public void Delete(Article article);
        public void SaveToDb();
    }
}
