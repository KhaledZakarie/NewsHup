using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NewsHup.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public  List<Article>  Articles { get; set; }   = new List<Article>();
    }
}
