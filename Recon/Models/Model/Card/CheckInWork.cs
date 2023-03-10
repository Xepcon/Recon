using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recon.Models.Model.Card
{
    public class CheckInWork
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public DateTime? Date { get; set; } = DateTime.Now;

        public string CardId { get; set; }


    }
}
