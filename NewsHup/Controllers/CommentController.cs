using Microsoft.AspNetCore.Mvc;
using NewsHup.Models;
using NewsHup.Repository;
using NewsHup.ViewModels;

namespace NewsHup.Controllers
{
    public class CommentController : Controller
    {
        ICommentRepository commentRepository;

        public CommentController(ICommentRepository _commentRepository)
        {
            commentRepository = _commentRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddComment(int articleId, int userId) //userId that will add comment
        {
            CommentWithUserViewModel commentWithUser = new CommentWithUserViewModel();
            commentWithUser.ArticleId = articleId;
            commentWithUser.CommenterId = userId;
            return View(commentWithUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(CommentWithUserViewModel PostedComment)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    Comment comment = new Comment();
                    comment.CommentText = PostedComment.CommentText;
                    comment.ArticleId = PostedComment.ArticleId;
                    comment.UserId = PostedComment.CommenterId;
                    commentRepository.Add(comment);

                    return RedirectToAction("ArticleDetils", "Article", new {id = PostedComment.ArticleId});
                }
                catch(Exception ex)
                {
                    return View(PostedComment);      
                }

            }
            return View(PostedComment);

        }
    }
}
