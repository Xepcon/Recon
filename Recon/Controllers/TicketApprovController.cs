using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.Group;
using Recon.Models.Model.Ticket;
using System.Diagnostics;

namespace Recon.Controllers
{
    public class TicketApprovController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly DataDbContext _dbContext;

        public TicketApprovController(IUserService userService, IGroupService groupService, DataDbContext dbContext)
        {
            _userService = userService;
            _groupService = groupService;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            if (_userService.IsAuthenticated())
            {
                if (_groupService.isGroupOwner())
                {
                    IEnumerable<IGroup> userGroupsPrincipal = _groupService.getUserGroup().Where(x => x.principalId == _userService.getUserId());

                    List<DayOffTicket> res = new List<DayOffTicket>();

                    foreach (var item in userGroupsPrincipal)
                    {
                        if (_dbContext.DayOffTicket.Where(x => x.groupId == item.groupId).Any())
                        {

                            res.AddRange(_dbContext.DayOffTicket.Where(x => x.groupId == item.groupId && x.isApproved == false).ToList());
                        }
                    }
                    Debug.WriteLine(res.Count);
                    return View(res);
                }
                else
                {
                    return RedirectToAction("Index", "DashBoard");
                }

            }

            return RedirectToAction("Login", "Account");

        }
        [HttpPost]
        public IActionResult Approve(int id)
        {
            if (_userService.IsAuthenticated())
            {
                if (_groupService.isGroupOwner())
                {
                    if (_dbContext.DayOffTicket.Where(x => x.Id == id).Any())
                    {
                        var tmp = _dbContext.DayOffTicket.Find(id);
                        tmp.isApproved = true;
                        _dbContext.SaveChanges();
                    }
                    return RedirectToAction("Index", "TicketApprov");
                }
                return RedirectToAction("Index", "DashBoard");
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
