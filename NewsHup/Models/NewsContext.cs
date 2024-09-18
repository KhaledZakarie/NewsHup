using Microsoft.EntityFrameworkCore;
using NewsHup.Models;

namespace TestMVC.Models
{
    public class NewsContext:DbContext
    {
        DbSet<User> Users { set; get; }
        DbSet<Article> Articles { set; get; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=NewsDB;Integrated Security=True;Trust Server Certificate=True");
        }
    }
}
