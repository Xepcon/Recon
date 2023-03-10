using System.ComponentModel.DataAnnotations;

namespace Recon.Models.Model.Account
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
