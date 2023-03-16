using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.Group;
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
        [HttpGet]
        public IActionResult UpdatePersonalInfo() {
            if (_userService.IsAuthenticated())
            {
                int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
                var person = _dbContext.Person.Where(x=>x.userId ==userId).FirstOrDefault();
                Debug.WriteLine(person);
                Debug.WriteLine("AAAAAAAAA");
                ViewBag.data = JsonConvert.SerializeObject(person); 
                if (person == null)
                {
                    return View("Error");
                }
                return View();
            }
            else {
                return View("Error");
            }                
          
        }
        [HttpPost]
        public IActionResult UpdatePersonalInfo(Person model)
        {                
            //Debug.WriteLine(model.ToString());
            Debug.WriteLine(model.FirstName);
            Debug.WriteLine(model.LastName);
            if (_userService.IsAuthenticated())
            {
                int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
              
                model.userId = userId;
                if (ModelState.IsValid)
                {
                    _dbContext.Update(model);
                    _dbContext.SaveChanges();
                    return View("Index");
                }
                return View(model);
            }
            else
            {
                return View("Error");
            }

        }

        public IActionResult DayOff() {

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
                dayOffTicket.userId = _userService.getUserId();
                _dbContext.DayOffTicket.Add(dayOffTicket);
                _dbContext.SaveChanges();
                return View("Index");
            }
            return RedirectToAction("Login", "Account");
            
        }
        public IActionResult AttendanceSheet()
        {
            if (_userService.IsAuthenticated())
            {
                int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
                var data = _dbContext.Attendances.Where(x => x.userId == userId);
                ViewBag.data = JsonConvert.SerializeObject(data);
                return View();

            }
            else {
                return RedirectToAction("Login", "Account");
            }
          
        }

        public IActionResult AttendanceSheetEdit(string id)
        {
            if (_userService.IsAuthenticated())
            {
                ViewBag.AttendanceId = id;
                
                int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
                int attendId = int.Parse(id);
                var userAttendence = _dbContext.Attendances.Where(x => x.AttendanceId == attendId).FirstOrDefault();
                ViewBag.AttendanceName = userAttendence.AttendanceName +"_"+ _userService.getUserName();
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

        public IActionResult TicketHistory() {
            if (_userService.IsAuthenticated()) {
                var list = _dbContext.DayOffTicket.Where(x => x.userId == _userService.getUserId());
                return View(list);
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UpdatePassword() { 

            return View();
            
        }

        [HttpPost]
        public IActionResult UpdatePassword(ChangePasswordViewModel model)
        {
            Debug.WriteLine("CALLED UPDATE PASS");


            Debug.WriteLine(model.ToString());
            if (model != null)
            {
                int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
                var user = _userService.GetById(userId);


                if (userId != null)
                {

                    if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.PasswordHash))
                    {
                        ModelState.AddModelError("OldPassword", "Old password is incorrect");
                        return View(model);
                    }

                    _userService.ChangePassword(userId, model.NewPassword);
                    _userService.LogOut();

                    // redirect the user to the login page
                    return RedirectToAction("Login", "Account");
                }
            }

            return View();
        }
    }
}
