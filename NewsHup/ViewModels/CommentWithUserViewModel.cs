using System.ComponentModel.DataAnnotations;

namespace NewsHup.ViewModels
{
    public class CommentWithUserViewModel
    {
        public int CommenterId { get; set; }
        public int ArticleId { get; set; }
        [MinLength(3)]
        public string CommentText { get; set; }
    }
}
