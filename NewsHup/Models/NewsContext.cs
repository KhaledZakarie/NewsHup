using Microsoft.EntityFrameworkCore;
using NewsHup.Models;

namespace TestMVC.Models
{
    public class NewsContext : DbContext
    {
        public DbSet<User> Users { set; get; }
        public DbSet<Article> Articles { set; get; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=NewsDB;Integrated Security=True;Trust Server Certificate=True");
        }
        //

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
           .HasOne(c => c.User)
           .WithMany(u => u.Comments)
           .HasForeignKey(c => c.UserId)
           .OnDelete(DeleteBehavior.Restrict); // Disable cascade delete



            modelBuilder.Entity<Article>()
           .HasOne(a => a.User)
           .WithMany(a => a.Articles)
           .HasForeignKey(a => a.UserId)
           .OnDelete(DeleteBehavior.Restrict);

          modelBuilder.Entity<Article>()
         .HasOne(a => a.Category)
         .WithMany(a => a.Articles)
         .HasForeignKey(a => a.CatId)
         .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
