using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Account;
using Recon.Models.Model.Ticket;
using Recon.ViewModel;
using System;
using System.Diagnostics;
using System.Drawing.Printing;

namespace Recon.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataDbContext _dbContext;
        private readonly IGroupService _groupService;

        public DashBoardController(IUserService userService, IHttpContextAccessor httpContextAccessor, DataDbContext dbContext, IGroupService groupService) {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _groupService = groupService;
        }

        public IActionResult UpdateWorkingHour()
        {

            return View();

        }
        
        public IActionResult CheckHistory()
        {

            return View();
        }
       

        /*public IActionResult DayOff() {

            ViewBag.UserGroup = JsonConvert.SerializeObject(_groupService.getUserGroup());
            return View();
        }
        [HttpPost]
        public IActionResult DayOff(DayOffTicket model)
        {
           
            DayOffTicket dayOffTicket = new DayOffTicket();

            if (_userService.IsAuthenticated()) {
                dayOffTicket.StartDayOff = model.StartDayOff;
                dayOffTicket.EndDayOff = model.EndDayOff;
                dayOffTicket.Description = model.Description;
                dayOffTicket.Title = model.Title;
                dayOffTicket.Created = DateTime.Now;
                dayOffTicket.Updated = DateTime.Now;
                dayOffTicket.isApproved = false;
                dayOffTicket.groupId = model.groupId;
                dayOffTicket.userId = _userService.GetUserId();
                _dbContext.DayOffTicket.Add(dayOffTicket);
                _dbContext.SaveChanges();
                return View("Index");
            }
            return RedirectToAction("Login", "Account");
            
        }*/
        [CustomRole("Intern")]
        public IActionResult AttendanceSheet()
        {
            if (_userService.IsAuthenticated())
            {
                int userId = _userService.GetUserId();
                var data = _dbContext.Attendances.Where(x => x.userId == userId);
                ViewBag.data = JsonConvert.SerializeObject(data);
                return View();

            }
            else {
                return RedirectToAction("Login", "Account");
            }
          
        }

        [CustomRole("Intern")]
        public IActionResult AttendanceSheetEdit(string id)
        {
            if (_userService.IsAuthenticated())
            {
                ViewBag.AttendanceId = id;

                int userId = _userService.GetUserId();
                int attendId = int.Parse(id);
                var userAttendence = _dbContext.Attendances.Where(x => x.AttendanceId == attendId).FirstOrDefault();
                if (userAttendence.isClosed == true)            
                    ViewBag.IsClosed = "false";
                else
                     ViewBag.IsClosed = "true";

                ViewBag.AttendanceName = userAttendence.AttendanceName +"_"+ _userService.GetUserName();
                if (userAttendence.userId != userId) {
                    //Error view 
                    return View("Error");
                }

                return View();

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

      
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        
    }
}
