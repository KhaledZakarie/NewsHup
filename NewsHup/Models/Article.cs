using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsHup.Models
{
    public class Article
    {
        public int Id { get; set; }

        public string ImageUrl {  get; set; }

        [MinLength(3)]
        public string Title { get; set; }
        [MinLength(20)]
        public string Content { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.Now;

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        public List<Comment>? Comments { get; set; } = new List<Comment>();
        [ForeignKey("Category")]

        public int CatId { get; set; }

        public Category? Category { get; set; }
    }
}
