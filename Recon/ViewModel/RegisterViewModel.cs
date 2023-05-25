﻿using System.ComponentModel.DataAnnotations;

namespace Recon.ViewModel
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Kérlek add meg az emailcímet !")]
        [EmailAddress(ErrorMessage = "Kérlek adj meg egy helyes email címet !")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /*[Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }*/



        [Required(ErrorMessage = "Kérlek add meg a felhasználónevet !")]
        [Display(Name = "Username")]
        public string Username { get; set; }




    }
}
