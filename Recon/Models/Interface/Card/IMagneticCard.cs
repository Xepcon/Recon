using System.ComponentModel.DataAnnotations;

namespace Recon.Models.Interface.Card
{
    public interface IMagneticCard
    {
        [Key]
        [Required]
        public string CardId { get; set; }

        [Key]
        [Required]
        public int userId { get; set; }
    }
}
