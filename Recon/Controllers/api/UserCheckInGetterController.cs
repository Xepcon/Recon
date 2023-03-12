using Microsoft.AspNetCore.Mvc;
using Recon.Data;
using Recon.Models.Model.Card;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recon.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCheckInGetterController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        // GET: api/<UserCheckInGetterController>
        
        public UserCheckInGetterController(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserCheckInGetterController>/5
        [HttpGet("{id}")]
      
        public IActionResult Get(string id)
        {
            if (id != null)
            {
                var cardid = _dbContext.magneticCards.Where(x => x.UserId == id).FirstOrDefault();
                if (cardid != null)
                {
                    var res = _dbContext.Checks.Where(x => x.CardId == cardid.CardId);
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
