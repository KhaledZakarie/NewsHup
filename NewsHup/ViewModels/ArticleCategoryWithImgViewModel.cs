using NewsHup.Models;
using System.ComponentModel.DataAnnotations;

namespace NewsHup.ViewModels
{
    public class ArticleCategoryWithImgViewModel
    {
        [MinLength(3)]
        public string Title { get; set; }
        [MinLength(20)]
        public string Content { get; set; }

        public int CatId { get; set; }
        public int UserId { get; set; }
        [Required]
        public IFormFile FormFile { get; set; }

        public List<Category> categories { get; set; } = new List<Category>();

    }
}
