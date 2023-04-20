using Microsoft.AspNetCore.Mvc;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Ticket;
using Recon.Models.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recon.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketDayOffDataDatesController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly ITicketRepository _ticketRepository;

        public TicketDayOffDataDatesController(DataDbContext dbContext, IUserService userService, IGroupService groupService, ITicketRepository ticketRepository)
        {
            _dbContext = dbContext;
            _userService = userService;
            _groupService = groupService;
            _ticketRepository = ticketRepository;
        }
        // GET: api/<TicketDayOffDataDatesController>
        [HttpGet]
        public IActionResult Get()
        {

            if (_groupService.IsGroupOwner())
            {
                IEnumerable<IGroup> userGroupsPrincipal = _groupService.getUserGroup().Where(x => x.principalId == _userService.GetUserId());

                List<DayOffTicket> res = _ticketRepository.GetAllTicketsForPrincipal(userGroupsPrincipal);
                var filteredTickets = res.Select(ticket => new
                {
                    text = ticket.Title + " " + _userService.GetFullName(ticket.userId),
                    startDate = ticket.StartDayOff.ToString("yyyy-MM-ddTHH:mm:ss"),
                    endDate = ticket.EndDayOff.ToString("yyyy-MM-ddTHH:mm:ss"),
                    color = ticket.isApproved ? "#727bd2" : "#32c9ed",
                    approved = ticket.isApproved,
                    description = ticket.Description
                }).Where(ticket => ticket.approved == true)
                              .ToList();
                return Ok(filteredTickets);


            }
            else
            {
                return BadRequest();
            }
        }

        // GET api/<TicketDayOffDataDatesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


    }
}
