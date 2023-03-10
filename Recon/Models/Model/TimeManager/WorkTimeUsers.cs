using System.ComponentModel.DataAnnotations;

namespace Recon.Models.Model.TimeManager
{
    public class WorkTimeUsers
    {
        [Key]
        public string UserId { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string LunchStartTime { get; set; }
        public string LunchEndTime { get; set; }
    }
}
