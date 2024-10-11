using NewsHup.Models;
using System.ComponentModel.DataAnnotations;

namespace NewsHup.ViewModels
{
    public class ArticleWithUserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public List<Article> Articles { get; set; } = new List<Article>();

        //Edit article

        [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
        public string Title { get; set; }
        [MinLength(20, ErrorMessage = "Content must be at least 20 characters long")]
        public string Content { get; set; }
        public int CatId { get; set; }
        public string? ImageUrl { get; set; }

        //add article
        public IFormFile? FormFile { get; set; }

    }
}
