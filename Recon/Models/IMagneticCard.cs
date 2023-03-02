using System.ComponentModel.DataAnnotations;

namespace Recon.Models
{
    public interface IMagneticCard
    {
        [Key]
        [Required]
        public string CardId { get; set; }

        [Key]
        [Required]
        public string UserId { get; set; }
    }
}
