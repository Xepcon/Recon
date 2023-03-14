using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.ViewModel;
using System.Diagnostics;
using System.Drawing.Printing;

namespace Recon.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataDbContext _dbContext;

        public DashBoardController(IUserService userService, IHttpContextAccessor httpContextAccessor, DataDbContext dbContext) {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public IActionResult UpdateWorkingHour()
        {

            return View();

        }

        public IActionResult CheckHistory()
        {
            return View();
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
