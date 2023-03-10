using System.ComponentModel.DataAnnotations;

namespace Recon.Models.Model.Account
{
    public class UsersInRoles
    {
        [Key]
        public int userId { get; set; }

        [Key]
        public int roleId { get; set; }
    }
}
