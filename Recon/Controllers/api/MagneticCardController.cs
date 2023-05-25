using Microsoft.AspNetCore.Mvc;
using Recon.Data;
using Recon.Models.Model.Card;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recon.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MagneticCardController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        public MagneticCardController(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<MagneticCardController>
        [HttpGet]
        public string Get()
        {
            return "Api working";
        }


        // POST api/<MagneticCardController>
        [HttpPost]
        public string Post(string apiKey, string cardId)
        {
            
            Debug.WriteLine("cardId " + cardId);
            Debug.WriteLine("apiKey " + apiKey);
            if (apiKey == null || apiKey!="TEST" || cardId==null)
            {
                return null;
            }
            if(_dbContext.magneticCards.Where(x=>x.CardId==cardId).Any())
            {
                
                CheckInWork entry = new CheckInWork();

                entry.CardId = cardId;
                _dbContext.Checks.Add(entry);
                _dbContext.SaveChanges();
                return "Valid Card";
            }
            return "Invalid Card";

        }




    }
}
