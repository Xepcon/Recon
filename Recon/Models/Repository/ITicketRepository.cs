using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Ticket;

namespace Recon.Models.Repository
{
    public interface ITicketRepository
    {
        void CreateTicket(DayOffTicket model);
      
        bool ApproveTicket(int id);

        IEnumerable<DayOffTicket> GetUsersTicket(int userid);

        List<DayOffTicket> GetAllTicketsForPrincipal(IEnumerable<IGroup> Groups);

    
    }
}
