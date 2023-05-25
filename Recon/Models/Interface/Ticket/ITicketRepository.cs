using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Ticket;

namespace Recon.Models.Repository
{
    public interface ITicketRepository
    {
        /// Create a Ticket
        void CreateTicket(DayOffTicket model);
        /// Approve a ticket 
        bool ApproveTicket(int id);
        /// Get users ticket by id 
        IEnumerable<DayOffTicket> GetUsersTicket(int userid);
        /// get all ticket for the principal at the group  (Merging ticket)
        List<DayOffTicket> GetAllTicketsForPrincipal(IEnumerable<IGroup> Groups);


    }
}
