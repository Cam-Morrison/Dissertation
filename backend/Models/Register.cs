using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.model
{
    public partial class Register
    {
        [Required, MinLength(5, ErrorMessage = "User name be more than 5 characters."), 
        MaxLength(16, ErrorMessage = "User name be less than 16 characters.")]
        public string UserName { get; set; } = string.Empty;
        [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters."),
        MaxLength(32, ErrorMessage = "Password must be less than 32 characters.")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}