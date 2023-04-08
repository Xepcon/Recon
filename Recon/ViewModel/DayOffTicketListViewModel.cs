using Recon.Models.Model.Ticket;

namespace Recon.ViewModel
{
    public class DayOffTicketListViewModel
    {
        public IEnumerable<DayOffTicket> Tickets { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
    }
}
