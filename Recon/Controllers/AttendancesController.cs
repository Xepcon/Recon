using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Recon.Data;
using Recon.Models.Model.TimeManager;

namespace Recon.Controllers
{
    public class AttendancesController : Controller
    {    
        private readonly DataDbContext _dbContext;

        public AttendancesController(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            if (_dbContext.Attendances.ToList() != null)
            {
                ViewBag.data = JsonConvert.SerializeObject(_dbContext.Attendances.ToList());

            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
           
            if (id != null )
            {
                try
                {
                    var model = _dbContext.Attendances.Where(x => x.AttendanceId == id).FirstOrDefault();
                    if (model != null)
                    {
                        _dbContext.Attendances.Remove(model);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception e)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }


        }
        // GET: Attendances/Create
        public IActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        
        public IActionResult Create( Attendance attendance)
        {
            if (ModelState.IsValid)
            {

                var user = _dbContext.Person.Where(x => x.userId == attendance.userId).FirstOrDefault();
                if (user != null)
                {
                    _dbContext.Add(attendance);
                    _dbContext.SaveChanges();
                    for (int i = 1; i <= DateTime.DaysInMonth(attendance.CreatedAt.Year, attendance.CreatedAt.Month); i++)
                    {
                        AttendanceEntity tmp = new AttendanceEntity();
                        tmp.AttendanceId = attendance.AttendanceId;
                        Debug.WriteLine(attendance.AttendanceId);
                        tmp.Hour = null;

                        tmp.AttendanceDate = new DateTime(attendance.CreatedAt.Year, attendance.CreatedAt.Month, i);
                        tmp.isWeekend = tmp.AttendanceDate.DayOfWeek == DayOfWeek.Saturday || tmp.AttendanceDate.DayOfWeek == DayOfWeek.Sunday;
                        tmp.approved = false;
                        tmp.Minutes = null;
                        tmp.interName = user.FirstName + " " + user.LastName;
                        _dbContext.AttendanceEntitys.Add(tmp);
                    }
                    _dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else {
                    return BadRequest("No user");
                }
            }
            return View(attendance);
        }

        
    }
}
