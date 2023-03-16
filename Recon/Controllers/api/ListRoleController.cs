using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;

namespace Recon.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListRoleController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        private readonly IUserService _userService;
        public ListRoleController(DataDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }
        [HttpGet]
        public ActionResult<List<Roles>> Get()
        {
            if (!_userService.IsAuthenticated())
            {
                return Unauthorized();
            }

            var data = _dbContext.Role.ToList();

            return data;
        }

    }
}
