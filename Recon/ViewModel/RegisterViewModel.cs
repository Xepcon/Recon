using System.ComponentModel.DataAnnotations;

namespace Recon.ViewModel
{
    public class RegisterViewModel
    {

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /*public RegisterViewModel()
        {
            Email = ""; // set default value
            UserName = "";
            Password = "";
            ConfirmPassword = "";
        }*/
    }
}
