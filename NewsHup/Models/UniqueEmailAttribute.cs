using System.ComponentModel.DataAnnotations;
using TestMVC.Models;

namespace NewsHup.Models
{
    internal class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Ensure email is not null or empty
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult("Email is required.");
            }

            // Resolve NewsContext from DI container using ValidationContext
            var context = validationContext.GetService(typeof(NewsContext)) as NewsContext;

            // Check if the context is null
            if (context == null)
            {
                return new ValidationResult("Unable to access database context.");
            }

            // Get the current user object from validationContext
            var userFromRes = validationContext.ObjectInstance as User;

            // Ensure the user object is not null
            if (userFromRes == null)
            {
                return new ValidationResult("User object is invalid.");
            }

            // Find if there are any other users with the same email (excluding the current user)
            var user = context.Users.FirstOrDefault(u => u.Email == value.ToString() && u.Id != userFromRes.Id);

            // If email is already used by another user
            if (user != null)
            {
                return new ValidationResult("Email already exists.");
            }

            // If no issues are found
            return ValidationResult.Success;
        }
    }
}
