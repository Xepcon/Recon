using System.ComponentModel.DataAnnotations;

namespace Recon.Models.Model.TimeManager
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get;set; }

        public string AttendanceName { get;set;}

        public DateTime CreatedAt { get;set; } = DateTime.Now;

        public int userId { get; set; }

    }
}
