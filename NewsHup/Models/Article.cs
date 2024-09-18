using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsHup.Models
{
    public class Article
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }
        public string Content { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }


    }
}
