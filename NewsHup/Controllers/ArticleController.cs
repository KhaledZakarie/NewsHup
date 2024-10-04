using Microsoft.AspNetCore.Mvc;
using NewsHup.Models;
using NewsHup.Repository;
using NewsHup.ViewModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NewsHup.Controllers
{
    public class ArticleController : Controller
    {
        IArticleRepository articleRepository;
        ICategoryRepository categoryRepository;
        IUserRepository userRepository;
        IHostingEnvironment hosting;
        public ArticleController(IArticleRepository _articleRepository, ICategoryRepository _categoryRepository,IUserRepository _userRepository , IHostingEnvironment _hosting)
        {
            articleRepository = _articleRepository;
            categoryRepository = _categoryRepository;
            userRepository = _userRepository;
            hosting = _hosting;
        }


        public IActionResult ArticleIndex()
        {
            List<Article> articles = articleRepository.GetAll();
            return View(articles);
        }



        public IActionResult AddArticle(int userId)
        {
            ArticleCategoryWithImgViewModel articleCategory = new ArticleCategoryWithImgViewModel();
            articleCategory.UserId = userId;
            articleCategory.categories = categoryRepository.GetAll();

            return View(articleCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddArticle(ArticleCategoryWithImgViewModel newArticle)
        {
            if(ModelState.IsValid)
            {
                try
                {

                    Article article = new Article();
                    //Upload Img
                    
                    if(newArticle.FormFile != null)///it upload image
                    {
                        string FileName = newArticle.FormFile.FileName;
                        string uploadFolder = Path.Combine(hosting.WebRootPath, "upload");
                        string FullPath = Path.Combine(uploadFolder, FileName);
                        newArticle.FormFile.CopyTo(new FileStream(FullPath, FileMode.Create));
                        article.ImageUrl = FileName;
                    }
                    else //put defualt img if not upload img
                    {
                        article.ImageUrl = "https://cdn.vectorstock.com/i/1000x1000/50/49/colorful-newspaper-article-design-vector-10795049.webp";
                    }

                    
                    article.Title = newArticle.Title;
                    article.Content = newArticle.Content;
                    article.CatId = newArticle.CatId;
                    
                    
                    /*temp*/article.UserId = 1;
                    //article.UserId = newArticle.UserId;
                    articleRepository.AddArticle(article);

                    return RedirectToAction("ArticleIndex");
                }
                catch(Exception ex)
                {
                    newArticle.categories = categoryRepository.GetAll();
                    return View(newArticle);
                }
                
            }
            newArticle.categories = categoryRepository.GetAll();
            return View(newArticle);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                Article article = articleRepository.GetArticleBy(a => a.Id == id);
                articleRepository.Delete(article);

                return RedirectToAction("MyArticles", new { userId = article.UserId });
            }
            catch(Exception ex)
            {
                Article article = articleRepository.GetArticleBy(a => a.Id == id);
                return RedirectToAction("MyArticles", new { userId = article.UserId });
            }
            
        }


        public IActionResult MyArticles(int userId)
        {
            if(userId != 0)
            {
                //Get the Data 
                User user = userRepository.GetUserBy(u => u.Id == userId);
                List<Article> articles = articleRepository.GetArticlesBy(a => a.UserId == userId);

                //Maping
                ArticleWithUserViewModel articleWithUser = new ArticleWithUserViewModel();
                articleWithUser.UserId = userId;
                articleWithUser.UserName = user.Name;
                articleWithUser.Articles = articles;

                return View(articleWithUser);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Edit(int id)
        {
            Article article = articleRepository.GetArticleBy(a => a.Id == id);
            ViewData["Categories"] = categoryRepository.GetAll();
            return View(article);
        }

        
    }
}
