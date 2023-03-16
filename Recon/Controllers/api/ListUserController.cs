using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;

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
        public ListUserController(DataDbContext dbContext, IUserService userService) {
            _dbContext= dbContext;
            _userService= userService;
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

      
    }
}
