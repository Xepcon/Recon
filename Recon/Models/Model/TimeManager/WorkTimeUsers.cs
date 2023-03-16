
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recon.Models.Model.TimeManager
{
    public class WorkTimeUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

       
    }
}
