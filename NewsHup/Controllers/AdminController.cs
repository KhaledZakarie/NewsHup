using Microsoft.AspNetCore.Mvc;
using NewsHup.Models;
using TestMVC.Models;  // Assuming NewsContext is in this namespace
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NewsHup.Enums;

namespace NewsHup.Controllers
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class AdminController : Controller
    {
        private readonly NewsContext _context;

        // Inject the NewsContext using dependency injection
        public AdminController(NewsContext context)
        {
            _context = context;
        }

        // Dashboard action
        public async Task<IActionResult> Dashboard()
        {
            // Fetch total counts for categories, articles, and users
            var totalCategories = await _context.Categories.CountAsync();
            var totalArticles = await _context.Articles.CountAsync();
            var totalUsers = await _context.Users.CountAsync();

            // Pass the counts to the view using ViewBag
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalArticles = totalArticles;
            ViewBag.TotalUsers = totalUsers;

            // Fetch article statistics for each month over multiple years
            // Grouping articles by month and year, counting them
            var articleStatistics = await _context.Articles
                .GroupBy(a => new { a.PublishDate.Month, a.PublishDate.Year })  // Group by month and year
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month) // Order by year first, then by month
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Count = g.Count()
                })
                .ToListAsync();

            // Create arrays for the labels (e.g., "Jan 2023", "Feb 2024", etc.) and data (e.g., article counts)
            var labels = articleStatistics.Select(a => $"{new DateTime(a.Year, a.Month, 1):MMM yyyy}").ToArray();
            var data = articleStatistics.Select(a => a.Count).ToArray();

            // Pass the article statistics to the view using ViewBag
            ViewBag.ArticleStatisticsLabels = labels;
            ViewBag.ArticleStatisticsData = data;

            return View();
        }




        //*****************************************************************************//

        // Fetch Users from the database and pass them to the view
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync(); // Fetch all users asynchronously
            return View(users);  // Pass the list of users to the view
        }

        // POST request to add a new user
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return Json(new { success = false, message = "Email already exists." });
            }

            if (ModelState.IsValid)
            {
                // Add the user to the database without password hashing
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "User added successfully!" });
            }

            // Collect all validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            return Json(new { success = false, message = "Invalid data!", errors });
        }

        [HttpPost]
        public JsonResult CheckEmailUnique(string email)
        {
            // Check if any user with the provided email exists in the database
            bool isUnique = !_context.Users.Any(u => u.Email == email);

            // Return the result as JSON
            return Json(new { isUnique = isUnique });
        }


        // Delete user
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);  // Use FindAsync for better performance
            if (user == null)
            {
                return Json(new { success = false, message = "User not found!" }, 404);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "User deleted successfully!" });
        }

        // Fetch user details for editing
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" }, 404);
            }
            return Json(user);
        }

        // Edit user
        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser == null)
                {
                    return Json(new { success = false, message = "User not found" }, 404);
                }

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;

                // Only update the password if a new one is provided
                if (!string.IsNullOrEmpty(user.Password))
                {
                    existingUser.Password = user.Password;  // No hashing, direct assignment
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }



            // Collect detailed validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, message = "Invalid data", errors });
        }





        //**********************************************************************************//

        // Category Management View
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.Categories.ToListAsync();  // Fetch categories from the database
            return View(categories);  // Pass categories to the view
        }

        // POST: Add Category
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            // Check if the category name already exists
            bool categoryExists = _context.Categories.Any(c => c.CategoryName == category.CategoryName);

            if (categoryExists)
            {
                return Json(new { success = false, message = "Category name already exists." });
            }

            if (ModelState.IsValid)
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return Json(new { success = true, categoryId = category.CategoryId });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
        }



        // POST: Delete Category
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return Json(new { success = false, message = "Category not found" });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Category deleted successfully" });
        }

        // Get Category by ID for Editing
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return Json(new { success = false, message = "Category not found" });
            }

            // Return only the necessary properties
            return Json(new { success = true, category = new { category.CategoryId, category.CategoryName } });
        }



        // Edit Category
        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category)
        {
            // Check if the category name already exists (excluding the current category being edited)
            bool categoryExists = _context.Categories.Any(c => c.CategoryName == category.CategoryName && c.CategoryId != category.CategoryId);

            if (categoryExists)
            {
                return Json(new { success = false, message = "Category name already exists." });
            }

            if (ModelState.IsValid)
            {
                var existingCategory = await _context.Categories.FindAsync(category.CategoryId);
                if (existingCategory == null)
                {
                    return Json(new { success = false, message = "Category not found" });
                }

                existingCategory.CategoryName = category.CategoryName;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
        }


        //**********************************************************************************//
        // GET: Articles Management
        public async Task<IActionResult> AdminArticles()
        {
            var articles = await _context.Articles
                                         .Include(a => a.Category)
                                         .Include(a => a.User)  // Include user data
                                         .ToListAsync();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Users = await _context.Users.ToListAsync();

            return View("AdminArticles", articles); // Return the view explicitly with articles
        }

        // POST: Add Article
        [HttpPost]
        public async Task<IActionResult> AddArticle(Article article, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                );

                return Json(new { success = false, errors });
            }

            // Handle image upload
            if (Image != null && Image.Length > 0)
            {
                // Generate a unique file name
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
                var fileName = Path.GetFileNameWithoutExtension(Image.FileName) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(Image.FileName);
                var fullPath = Path.Combine(imagePath, fileName);
                // Save the file to the server
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                // Save the image file name (or path) in the database
                article.ImageUrl = "/upload/" + fileName;
            }
            else
            {
                article.ImageUrl = "/upload/DefaultImage.jpg"; // Default image if no image is uploaded
            }

            // Assign a default publish date if none is provided
            if (article.PublishDate == default(DateTime))
            {
                article.PublishDate = DateTime.Now;
            }

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return Json(new { success = true, articleId = article.Id });
        }


        // POST: Edit Article
        [HttpPost]
        public async Task<IActionResult> EditArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors in JSON format
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                );

                return Json(new { success = false, errors });
            }

            var existingArticle = await _context.Articles.FindAsync(article.Id);
            if (existingArticle == null)
            {
                return Json(new { success = false, message = "Article not found" });
            }

            // Update the existing article's fields
            existingArticle.Title = article.Title;
            existingArticle.Content = article.Content;
            existingArticle.CatId = article.CatId;
            if (!string.IsNullOrEmpty(article.ImageUrl))
            {
                existingArticle.ImageUrl = article.ImageUrl;
            }
            // Update the PublishDate if a new one is provided (avoid updating if it's the default)
            if (article.PublishDate != default(DateTime))
            {
                existingArticle.PublishDate = article.PublishDate;
            }

            _context.Update(existingArticle);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


        // GET: Get Article by ID
        public async Task<IActionResult> GetArticle(int id)
        {
            try
            {
                var article = await _context.Articles
                                            .Include(a => a.Category)
                                            .Include(a => a.User)
                                            .FirstOrDefaultAsync(a => a.Id == id);

                if (article == null)
                {
                    return Json(new { success = false, message = "Article not found" });
                }

                // Return structured JSON data
                return Json(new
                {
                    success = true,
                    article = new
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Content = article.Content,
                        CatId = article.CatId,
                        UserId = article.UserId,
                        ImageUrl = article.ImageUrl,
                        PublishDate = article.PublishDate
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error fetching article: " + ex.Message });
            }
        }


        // POST: Delete Article
        [HttpPost]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                var article = await _context.Articles.FindAsync(id);
                if (article == null)
                {
                    return Json(new { success = false, message = "Article not found" });
                }

                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting article: " + ex.Message });
            }
        }



    }
}