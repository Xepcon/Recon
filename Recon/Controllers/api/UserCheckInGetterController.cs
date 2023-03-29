using Microsoft.AspNetCore.Mvc;
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Model.Card;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recon.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UserCheckInGetterController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        private readonly IUserService _userService;
        // GET: api/<UserCheckInGetterController>

        public UserCheckInGetterController(DataDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService; 
        }

        [HttpGet("daytime/{id}")]
        
        public IActionResult GetTimeTable(string id)
        {
            if (!_userService.IsAuthenticated())
            {
                return Unauthorized();
            }
            if (id != null)
            {
                int Userid = int.Parse(id);
             
                    var WrokTimeEntitiy = _dbContext.WorkTimeUsers.Where(x => x.UserId == Userid).FirstOrDefault();
                    return Ok(WrokTimeEntitiy);
                
            }
            else
            {
                return BadRequest("Id null");
            }

        }
        // GET api/<UserCheckInGetterController>/5
        [HttpGet("{id}")]
      
        public IActionResult Get(string id, DateTime startDate, DateTime endDate)
        {
            if (!_userService.IsAuthenticated())
            {
                return Unauthorized();
            }
            if (id != null)
            {
                var cardid = _dbContext.magneticCards.Where(x => x.UserId == id).FirstOrDefault();
                if (cardid != null)
                {
                    var res = _dbContext.Checks.Where(x => x.CardId == cardid.CardId && x.Date >= startDate && x.Date <= endDate);
                    if (res != null)
                    {
                        return Ok(res);
                    }
                    else
                    {
                        return NotFound("No data");
                    }

                }
                return NotFound("Error");

            }
            else {
                return BadRequest("Id null");
            }
            


        }

       
    }
}
