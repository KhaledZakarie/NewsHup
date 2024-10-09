using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NewsHup.Repository;
using TestMVC.Models;

namespace NewsHup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                CookieAuthenticationDefaults.AuthenticationScheme,

                options =>
                {
                    options.LoginPath = "/User/Login";
                });
           



            // Register NewsContext for dependency injection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Register NewsContext for dependency injection with retry on failure
            builder.Services.AddDbContext<NewsContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    // Enable retry on transient errors
                    sqlOptions.EnableRetryOnFailure();
                })
            );





            var app = builder.Build();

            



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();    

            app.UseAuthorization();

            // Admin-specific route
            app.MapControllerRoute(
                name: "admin",
                pattern: "admin/{action=Dashboard}/{id?}",
                defaults: new { controller = "Admin", action = "Dashboard" });

            // Existing default route

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
