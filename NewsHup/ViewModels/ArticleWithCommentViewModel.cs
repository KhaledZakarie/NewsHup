using NewsHup.Models;

namespace NewsHup.ViewModels
{
    public class ArticleWithCommentViewModel
    {
        public int ArticleId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime PublishDate { get; set; }
        public List<Comment>? Comments { get; set; } = new List<Comment>();
        public string? CategoryName { get; set; }
        public string? AuthorName { get; set; }
        public int? AuthorId { get; set; }

        public string? NewComment { get; set; }
        public int? CommenterID { get; set; }
    }
}
