using System.ComponentModel.DataAnnotations;

namespace Recon.ViewModel
{
    public class ChangePasswordViewModel
    {       
        [Required]
        //[DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        //[DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
