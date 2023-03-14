
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Recon.Data;
using Recon.Models.Model.TimeManager;
using System.Diagnostics;


namespace Recon.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateAttendanceController : ControllerBase
    {
        private readonly DataDbContext _dbContext;
     

        public UpdateAttendanceController(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id}")]

        public IActionResult Get(string id) {
            
            if (id != null)
            {
                int intId = int.Parse(id);
                var res = _dbContext.AttendanceEntitys.Where(x => x.AttendanceId == intId);
                if (!res.IsNullOrEmpty())
                {
                    return Ok(res);
                }
                else {
                    return BadRequest("No Data");
                }
            
            }
            else {
                return BadRequest("Id null");
            }
        }
        
        [HttpPut("{id}")]        
        public IActionResult UpdateAttendance(int id, AttendanceEntity updatedAttendance)
        {
            Debug.WriteLine("CALLED FUKIN PUT");
            // Find the attendance record with the specified id
            Attendance attendance = _dbContext.Attendances.Find(id);
            Debug.WriteLine("Date: " + updatedAttendance.AttendanceDate);
            Debug.WriteLine("Hour: " + updatedAttendance.Hour);
            Debug.WriteLine("Minutes: " + updatedAttendance.Minutes);
            Debug.WriteLine("isWeekend: " + updatedAttendance.isWeekend);
            Debug.WriteLine("interName: " + updatedAttendance.interName);
            Debug.WriteLine("approved: " + updatedAttendance.approved);
            // If the attendance record doesn't exist, return a 404 error
            if (attendance == null)
            {
                return NotFound();
            }
            AttendanceEntity updateEntity = _dbContext.AttendanceEntitys.Where(x => x.AttendanceId == id && x.AttendanceDate == updatedAttendance.AttendanceDate).FirstOrDefault();
            if(updateEntity != null)
            {
                
                updateEntity.Hour = updatedAttendance.Hour;
                updateEntity.Minutes = updatedAttendance.Minutes;
                _dbContext.SaveChanges();

            }
            
            return Ok();
        }
    }
}
