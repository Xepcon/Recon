using Microsoft.AspNetCore.Mvc;
using Recon.Data;
using Recon.Models.Model.Card;
using Recon.Models.Model.TimeManager;
using System.Diagnostics;

namespace Recon.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateWorkHourController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        public UpdateWorkHourController(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

       
        [HttpGet]
        [Route("getWorkTime")]
        public WorkTimeUsers getWorkTime(string userid)
        {
            
            int user = int.Parse(userid);

            if(_dbContext.WorkTimeUsers.Find(user)!= null)
            {
                return _dbContext.WorkTimeUsers.Find(user);
            }
            return null;
        }


       
        [HttpPost]
        [Route("SaveWorkTime")]
        public void SaveWorkTime(WorkTimeUsers data )
        {
            Debug.WriteLine("ID : "+data.UserId);
            Debug.WriteLine(data.ToString());
            if (_dbContext.WorkTimeUsers.Find(data.UserId) == null)
            {
                _dbContext.WorkTimeUsers.Add(data);
                _dbContext.SaveChanges();
            }
            else {
                ///Pending Boss allow
                var ent = _dbContext.WorkTimeUsers.Find(data.UserId);
                _dbContext.WorkTimeUsers.Remove(ent);
                _dbContext.Add(data);
                _dbContext.SaveChanges();
            }
            
           
        }
    }
}
