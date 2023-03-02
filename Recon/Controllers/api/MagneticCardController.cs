using Microsoft.AspNetCore.Mvc;
using Recon.Data;
using Recon.Models;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recon.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MagneticCardController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        public MagneticCardController(DataDbContext dbContext) {
            _dbContext = dbContext;
        }

        // GET: api/<MagneticCardController>
        [HttpGet]
        public string Get()
        {
            return "WORKING";
        }

       
        // POST api/<MagneticCardController>
        [HttpPost]
        public void Post(string apiKey, string cardId)
        {
            Debug.WriteLine("CALLED FROM ESP");
            Debug.WriteLine("cardId "+cardId);
            Debug.WriteLine("apiKey " + apiKey);
            CheckInWork entry = new CheckInWork();
            
            entry.CardId = cardId;
            _dbContext.Checks.Add(entry);
            _dbContext.SaveChanges();
        }

        // POST api/<MagneticCardController>
        /*[HttpPost]
        public void Post1()
        {
            Debug.WriteLine("CALLED FROM ESP POST1");
          
        }*/



    }
}
