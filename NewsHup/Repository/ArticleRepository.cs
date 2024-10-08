﻿using NewsHup.Models;
using TestMVC.Models;

namespace NewsHup.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        //private NewsContext newsContext;

        private readonly NewsContext _newsContext;

        // Use Dependency Injection to inject NewsContext
        public ArticleRepository(NewsContext newsContext)
        {
            _newsContext = newsContext;
        }

        public void AddArticle(Article article)
        {
            _newsContext.Add(article);
            SaveToDb();
        }

        public void Edit(Article article)
        {
            _newsContext.Update(article);
            SaveToDb();
        }
        public void Delete(Article article)
        {
            _newsContext.Remove(article);
            SaveToDb();
        }

        public List<Article> GetAll()
        {
            List<Article> articles = _newsContext.Articles.ToList();
            return articles;
        }
        public Article GetArticleBy(Func<Article, bool> GetBy)
        {
            Article article = _newsContext.Articles.FirstOrDefault(GetBy);
            return article;
        }

        public List<Article> GetArticlesBy(Func<Article, bool> GetBy)
        {
            List<Article> articles = _newsContext.Articles.Where(GetBy).ToList();
            return articles;
        }

        public void SaveToDb()
        {
            _newsContext.SaveChanges();
        }
    }
}
