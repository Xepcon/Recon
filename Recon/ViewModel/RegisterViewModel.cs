using System.ComponentModel.DataAnnotations;

namespace Recon.ViewModel
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Kérlek add meg az emailcímet !")]
        [EmailAddress(ErrorMessage = "Kérlek adj meg egy helyes email címet !")]
        [Display(Name = "Email")]
        public string Email { get; set; }
       


        [Required(ErrorMessage = "Kérlek add meg a felhasználónevet !")]
        [Display(Name = "Username")]
        public string Username { get; set; }




    }
}
