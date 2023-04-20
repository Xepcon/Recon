
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Model.TimeManager;


namespace Recon.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]

    public class UpdateAttendanceController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
        private readonly IUserService _userService;

        public UpdateAttendanceController(DataDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        [HttpGet("{id}")]

        public IActionResult Get(string id)
        {

            if (!_userService.IsAuthenticated())
            {
                return Unauthorized();
            }

            if (id != null)
            {
                int intId = int.Parse(id);
                var res = _dbContext.AttendanceEntitys.Where(x => x.AttendanceId == intId);
                if (!res.IsNullOrEmpty())
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest("No Data");
                }

            }
            else
            {
                return BadRequest("Id null");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAttendance(int id, AttendanceEntity updatedAttendance)
        {
            if (!_userService.IsAuthenticated())
            {
                return Unauthorized();
            }
            //Debug.WriteLine("CALLED FUKIN PUT");
            // Find the attendance record with the specified id
            Attendance attendance = _dbContext.Attendances.Find(id);
            /*Debug.WriteLine("Date: " + updatedAttendance.AttendanceDate);
            Debug.WriteLine("Hour: " + updatedAttendance.Hour);
            Debug.WriteLine("Minutes: " + updatedAttendance.Minutes);
            Debug.WriteLine("isWeekend: " + updatedAttendance.isWeekend);
            Debug.WriteLine("interName: " + updatedAttendance.interName);
            Debug.WriteLine("approved: " + updatedAttendance.approved);*/
            // If the attendance record doesn't exist, return a 404 error
            if (attendance == null)
            {
                return NotFound();
            }
            AttendanceEntity updateEntity = _dbContext.AttendanceEntitys.Where(x => x.AttendanceId == id && x.AttendanceDate == updatedAttendance.AttendanceDate).FirstOrDefault();
            if (updateEntity != null)
            {

                updateEntity.Hour = updatedAttendance.Hour;
                updateEntity.Minutes = updatedAttendance.Minutes;
                _dbContext.SaveChanges();

            }

            return Ok();
        }
    }
}
