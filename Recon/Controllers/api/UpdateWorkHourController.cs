using Microsoft.AspNetCore.Mvc;
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Interface.Account;
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
        private readonly IUserService _userService;
        public UpdateWorkHourController(DataDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

       
        [HttpGet]
        [Route("getWorkTime")]
        public WorkTimeUsers getWorkTime(string userid)
        {
            if (!_userService.IsAuthenticated())
            {
                return null;
            }
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
           //_userService.IsAuthenticated())
            
             
            
            Debug.WriteLine("ID : "+data.UserId);
            Debug.WriteLine(data.ToString());
            if (_dbContext.WorkTimeUsers.Find(data.UserId) == null && _userService.IsAuthenticated())
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
