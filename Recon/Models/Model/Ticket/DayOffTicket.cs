using Recon.Models.Interface.Ticket;
using System.ComponentModel.DataAnnotations;

namespace Recon.Models.Model.Ticket
{
    public class DayOffTicket : ITicket
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int userId { get; set; }

        public DateTime StartDayOff { get; set; }

        public DateTime EndDayOff { get; set; }

        public bool isApproved { get; set; }

        public int groupId { get; set; }

    }
}
