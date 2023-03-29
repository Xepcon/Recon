using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.GroupLib;
using Recon.ViewModel;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recon.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ListUserController : ControllerBase
    {
        // GET: api/<ListUserController>
        private readonly DataDbContext _dbContext; 
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        public ListUserController(DataDbContext dbContext, IUserService userService,IGroupService groupService ) {
            _dbContext= dbContext;
            _userService= userService;
            _groupService= groupService;
        }
        [HttpGet]
        public ActionResult<List<ApiPersonViewModel>> Get()
        {
            if (!_userService.IsAuthenticated())
            {
                return Unauthorized();
            }

            var data = _dbContext.Person
                 .Select(p => new ApiPersonViewModel { Name = $"{p.FirstName} {p.LastName}", UserId = p.userId })
                 .ToList();

            return data;
        }
        
        [HttpGet]
        [Route("/ListUserForPrincipal")]
        public ActionResult<List<ApiPersonViewModel>> GetInters()
        {
            
            if (!_userService.IsAuthenticated())
            {
                return Unauthorized();
            }
            if (_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Admin" || r.Name == "Hr")) {
                var interId = _dbContext.Role.Where(x => x.Name == "Intern").FirstOrDefault();
                var inters = _dbContext.UsersInRole.Where(x => x.roleId == interId.Id).ToList();

                var data = _dbContext.Person
                    .Select(p => new ApiPersonViewModel { Name = $"{p.FirstName} {p.LastName}", UserId = p.userId })
                    .ToList();
                var res = data.Where(x => inters.Any(i => i.userId == x.UserId)).ToList();
                return res;

            }
            if (_groupService.IsGroupOwner())
            {

                List<int> groupids = _groupService.GetGroupIdByPrincipalId(_userService.GetUserId());
                var members = _dbContext.GroupMembers.Where(x => groupids.Contains(x.groupId)).Select(x => x.userId);

                var data = _dbContext.Person
                      .Where(p => members.Contains(p.userId))
                      .Select(p => new ApiPersonViewModel { Name = $"{p.FirstName} {p.LastName}", UserId = p.userId })
                      .ToList();
                var interId = _dbContext.Role.Where(x => x.Name == "Intern").FirstOrDefault();
                var inters = _dbContext.UsersInRole.Where(x => x.roleId == interId.Id).ToList();

                var res = data.Where(x => inters.Any(i => i.userId == x.UserId)).ToList();
                return res;
            }
            else {
                return Unauthorized();
            }
            
        }

        [HttpGet]
        [Route("/ListGroupMembersForPrincipal")]
        public ActionResult<List<ApiPersonViewModel>> getGroupMembers()
        {

            if (!_userService.IsAuthenticated())
            {
                return Unauthorized();
            }
            if (_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Admin" || r.Name == "Hr"))
            {
               
                
                var data = _dbContext.Person
                    .Select(p => new ApiPersonViewModel { Name = $"{p.FirstName} {p.LastName}", UserId = p.userId })
                    .ToList();
               
                return data;

            }
            if (_groupService.IsGroupOwner())
            {

                List<int> groupids = _groupService.GetGroupIdByPrincipalId(_userService.GetUserId());
                var members = _dbContext.GroupMembers.Where(x => groupids.Contains(x.groupId)).Select(x => x.userId);

                var data = _dbContext.Person
                      .Where(p => members.Contains(p.userId))
                      .Select(p => new ApiPersonViewModel { Name = $"{p.FirstName} {p.LastName}", UserId = p.userId })
                      .ToList();
              
                return data;
            }
            else
            {
                return Unauthorized();
            }

        }

    }
}
