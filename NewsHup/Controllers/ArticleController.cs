using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsHup.Models;
using NewsHup.Repository;
using NewsHup.ViewModels;
using System.Security.Claims;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NewsHup.Controllers
{
    public class ArticleController : Controller
    {
        IArticleRepository articleRepository;
        ICategoryRepository categoryRepository;
        IUserRepository userRepository;
        ICommentRepository commentRepository;
        IHostingEnvironment hosting;
        public ArticleController(IArticleRepository _articleRepository, ICategoryRepository _categoryRepository, IUserRepository _userRepository, ICommentRepository _commentRepository, IHostingEnvironment _hosting)
        {
            articleRepository = _articleRepository;
            categoryRepository = _categoryRepository;
            userRepository = _userRepository;
            hosting = _hosting;
            commentRepository = _commentRepository;
        }


        //public IActionResult ArticleIndex()
        //{
        //    List<Article> articles = articleRepository.GetAll();
        //    return View(articles);
        //}



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
            if (ModelState.IsValid)
            {
                try
                {

                    Article article = new Article();
                    //Upload Img

                    if (newArticle.FormFile != null)///img uploaded
                    {
                        string FileName = newArticle.FormFile.FileName;
                        string uploadFolder = Path.Combine(hosting.WebRootPath, "upload");
                        string FullPath = Path.Combine(uploadFolder, FileName);
                        newArticle.FormFile.CopyTo(new FileStream(FullPath, FileMode.Create));
                        article.ImageUrl = "/upload/" + FileName;
                    }
                    else //put defualt img if not upload img
                    {
                        article.ImageUrl = "/upload/DefualtNews.jpg";
                    }


                    article.Title = newArticle.Title;
                    article.Content = newArticle.Content;
                    article.CatId = newArticle.CatId;

                    //if userId =0 // mean not select user --> put him/her in general
                    if (newArticle.CatId == 0)
                    {
                        article.CatId = 1;
                    }

                    article.UserId = userRepository.GetLoggerId(HttpContext);

                    articleRepository.AddArticle(article);

                    return RedirectToAction("MyArticles", new { userId = User.FindFirst("Id").Value });
                }
                catch (Exception ex)
                {
                    newArticle.categories = categoryRepository.GetAll();
                    return View(newArticle);
                }

            }
            newArticle.categories = categoryRepository.GetAll();
            return View(newArticle);
        }



        //[HttpPost]
        //public IActionResult AddArticlePopUp(Article article, IFormFile? Image)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var errors = ModelState.ToDictionary(
        //            err => err.Key,
        //            err => err.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
        //        );
        //        return Json(new { success = false, errors });
        //    }

            

        //    if (Image != null)
        //    {
        //        // Generate a unique file name
        //        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
        //        var fileName = Path.GetFileNameWithoutExtension(Image.FileName) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(Image.FileName);
        //        var fullPath = Path.Combine(imagePath, fileName);

        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            Image.CopyTo(stream);
        //        }

        //        article.ImageUrl = "/upload/" + fileName;
        //    }
        //    else
        //    {
        //        article.ImageUrl = "/upload/DefualtNews.jpg"; // Default image 
        //    }

        //    articleRepository.AddArticle(article);
            
            
        //    return Json(new { success = true, articleId = article.Id });
        //}



        public IActionResult ArticleDetils(int id)
        {
            Article article = articleRepository.GetArticleBy(a => a.Id == id);
            User author = userRepository.GetUserBy(u => u.Id == article.UserId);
            Category category = categoryRepository.GetCategoryBy(c => c.CategoryId == article.CatId);
            List<Comment> comments = commentRepository.GetCommentsBy(c => c.ArticleId == article.Id);

            ArticleWithCommentViewModel articleWithComment = new ArticleWithCommentViewModel();
            articleWithComment.ArticleId = article.Id;
            articleWithComment.Title = article.Title;
            articleWithComment.Content = article.Content;
            articleWithComment.ImageUrl = article.ImageUrl;
            articleWithComment.CategoryName = category.CategoryName;
            articleWithComment.PublishDate = article.PublishDate;
            articleWithComment.AuthorName = author.Name;
            articleWithComment.AuthorId = author.Id;

            articleWithComment.Comments = comments;

            return View(articleWithComment);
        }
        public IActionResult Delete(int id)
        {
            try
            {
                Article article = articleRepository.GetArticleBy(a => a.Id == id);
                articleRepository.Delete(article);

                return RedirectToAction("MyArticles", new { userId = article.UserId });
            }
            catch (Exception ex)
            {
                Article article = articleRepository.GetArticleBy(a => a.Id == id);
                return RedirectToAction("MyArticles", new { userId = article.UserId });

            }

        }

        [Authorize]
        public IActionResult MyArticles()
        {
            int userId = userRepository.GetLoggerId(HttpContext);


            ViewData["Categories"] = categoryRepository.GetAll();

            ViewBag.Users = userRepository.GetAll();

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
        public IActionResult Edit(int id)
        {

            Article article = articleRepository.GetArticleBy(a => a.Id == id);
            ViewData["Categories"] = categoryRepository.GetAll();
            return View(article);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Article editedArticle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if userId =0 // mean not select user --> put him/her in general
                    if (editedArticle.CatId == 0)
                    {
                        editedArticle.CatId = 1;
                    }
                    articleRepository.Edit(editedArticle);

                    return RedirectToAction("MyArticles", new { userId = editedArticle.UserId });
                }
                catch (Exception ex)
                {
                    ViewData["Categories"] = categoryRepository.GetAll();
                    return View(editedArticle);
                }

            }
            ViewData["Categories"] = categoryRepository.GetAll();
            return View(editedArticle);
        }

        
        public IActionResult UsersArticles(int userId)
        {
            if(userId == userRepository.GetLoggerId(HttpContext))
            {
                return RedirectToAction("MyArticles");
            }


            ViewData["Categories"] = categoryRepository.GetAll();

            ViewBag.Users = userRepository.GetAll();

            //Get the Data 
            User user = userRepository.GetUserBy(u => u.Id == userId);
            List<Article> articles = articleRepository.GetArticlesBy(a => a.UserId == userId);

            //Maping
            ArticleWithUserViewModel articleWithUser = new ArticleWithUserViewModel();
            articleWithUser.UserId = userId;
            articleWithUser.UserName = user.Name;
            articleWithUser.Articles = articles;
            articleWithUser.UserImg = user.UserImage;



            return View(articleWithUser);

        }

    }
}