using System.ComponentModel.DataAnnotations;

namespace NewsHup.Models
{
    public class ArticleTag
    {
        [Key]
        public int ArticleId { get; set; }
        public int tagID { get; set; }
    }
}
