using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recon.Models.Model.TimeManager
{
    public class AttendanceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AttendanceId { get; set; }
        [Key]
        public DateTime AttendanceDate { get; set; }

        public int? Hour { get; set; }

        public int? Minutes { get; set; }

        public bool isWeekend { get; set; }

        public string? interName { get; set; }

        public bool approved { get; set; }

      
    }
}
