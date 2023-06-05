﻿using System.ComponentModel.DataAnnotations;

namespace MyWebApplicationCRUD.Models
{
    public class UserCredentials
    {
        [Key]
        public string? UserId { get; set; }

        
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        public string? UserType { get; set; }
    }
}
