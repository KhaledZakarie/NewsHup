﻿
using System.ComponentModel.DataAnnotations;
using TestMVC.Models;

namespace NewsHup.Models
{
    internal class UniqueEmailAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            NewsContext context = new NewsContext();
             User userFromRes =  validationContext.ObjectInstance as User;
            var user = context.Users.FirstOrDefault(u=>u.Email == value.ToString());

            if (user != null)
            {
                if (userFromRes.Name == null)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("Email already exists");
            }
            else 
            {
                if (userFromRes.Name == null)
                {
                    return new ValidationResult("Email already not exists");
                }
                return ValidationResult.Success;
            }



            
        }
    }
}