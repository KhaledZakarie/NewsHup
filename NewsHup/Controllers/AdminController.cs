﻿using Microsoft.AspNetCore.Mvc;
using NewsHup.Models;
using TestMVC.Models;  // Assuming NewsContext is in this namespace
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NewsHup.Controllers
{
    public class AdminController : Controller
    {
        private readonly NewsContext _context;

        // Inject the NewsContext using dependency injection
        public AdminController(NewsContext context)
        {
            _context = context;
        }

        // Dashboard action
        public IActionResult Dashboard()
        {
            return View();
        }

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


    }
}
