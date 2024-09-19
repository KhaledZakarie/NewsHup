using NewsHup.Enums;
using System.ComponentModel.DataAnnotations;

namespace NewsHup.Models
{
    public class User
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        //Minimum eight characters, at least one letter and one number
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")]
        public string Password { get; set; }

        public Role Role { get; set; }

        public List<Article> Articles{ get; set; } = new List<Article>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
