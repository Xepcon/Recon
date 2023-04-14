using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;
using Recon.Models.Model.GroupLib;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recon.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ListGroupController : ControllerBase
    {
        // GET: api/<ListUserController>
        private readonly DataDbContext _dbContext; 
        private readonly IUserService _userService;
        public ListGroupController(DataDbContext dbContext, IUserService userService) {
            _dbContext= dbContext;
            _userService= userService;
        }
        [HttpGet]
        public ActionResult<List<Group>> Get()
        {
            if (!_userService.IsAuthenticated())
            {
                return Unauthorized();
            }

            var data = _dbContext.Groups.ToList();

            return data;
        }

        [HttpGet]
        [Route("/ListOnlyGroupOwnedGroup")]
        public ActionResult<List<Group>> GetOnlyPrincipalGroup() {
            if (!_userService.IsAuthenticated())
            {
                return Unauthorized();
            }
            var userid = _userService.GetUserId();
            
            if (_userService.GetRolesForUser(userid).Any(r => r.Name == "Admin" || r.Name == "Hr"))
            {
                var data =  _dbContext.Groups.ToList();
                return data;
            }
            else {
                var data = _dbContext.Groups.Where(x => x.principalId == userid).ToList();
                return data;
            }
            

           
        }

      
    }
}
