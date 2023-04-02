using Recon.Data;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Ticket;

namespace Recon.Models.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DataDbContext _dbContext;
        private readonly IGroupService _groupService;

        public TicketRepository(DataDbContext dbContext, IGroupService groupService)
        {
            _dbContext = dbContext;
            _groupService = groupService;   
        }

        public bool ApproveTicket(int id)
        {
            if (_groupService.IsGroupOwner())
            {
                if (_dbContext.DayOffTicket.Where(x => x.Id == id).Any())
                {
                    var tmp = _dbContext.DayOffTicket.Find(id);
                    tmp.isApproved = true;
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }

        public void CreateTicket(DayOffTicket model)
        {
            
            _dbContext.DayOffTicket.Add(model);
            _dbContext.SaveChanges();
                
                
            }

        public List<DayOffTicket> GetAllTicketsForPrincipal(IEnumerable<IGroup> Groups)
        {
            List<DayOffTicket> res = new List<DayOffTicket>();

            foreach (var item in Groups)
            {
                if (_dbContext.DayOffTicket.Where(x => x.groupId == item.groupId).Any())
                {
                    //res.AddRange(_dbContext.DayOffTicket.Where(x => x.groupId == item.groupId && x.isApproved == false).ToList());
                    res.AddRange(_dbContext.DayOffTicket.Where(x => x.groupId == item.groupId).ToList());
                }
            }
            return res;
        }

        public IEnumerable<DayOffTicket> GetUsersTicket(int userid)
        {
            return _dbContext.DayOffTicket.Where(x=>x.userId==userid);
        }

  

    }
}
