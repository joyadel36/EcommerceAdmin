﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class SignUpAuthViewModel
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "you must entr your First name.....")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "you must entr your Last name.....")]
        public string LastName { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }

        [Required(ErrorMessage = "you must entr your phone number.....")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "you must entr your email.....")]
        public string Email { get; set; }

        [Required(ErrorMessage = "you must entr your user password.....")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "you must entr your user password.....")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
   
    }
}
