using System.ComponentModel.DataAnnotations;
using TestMVC.Models;

namespace NewsHup.Models
{
    internal class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Skip validation if no email is provided
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult("Email is required.");
            }

            var context = validationContext.GetService(typeof(NewsContext)) as NewsContext;

            if (context == null)
            {
                return new ValidationResult("Unable to access database context.");
            }

            var userFromRes = validationContext.ObjectInstance as User;

            // Skip email uniqueness validation if this is a login request (name is usually null during login)
            if (userFromRes?.Name == null)
            {
                return ValidationResult.Success;
            }

            // Check for email uniqueness in the context of registration
            var user = context.Users.FirstOrDefault(u => u.Email == value.ToString() && u.Id != userFromRes.Id);

            if (user != null)
            {
                return new ValidationResult("Email already exists.");
            }

            return ValidationResult.Success;
        }
    }
}
