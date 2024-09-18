using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsHup.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        [Required]
        public string CommentText { get; set; }

        public DateTime CommentDate { get; set; } = DateTime.Now; // Automatically set the date to now
    }
}
