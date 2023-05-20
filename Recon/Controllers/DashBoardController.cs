using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;

namespace Recon.Controllers
{
    [Authenticated]
    public class DashBoardController : Controller
    {
        private readonly IUserService _userService;
       
        private readonly DataDbContext _dbContext;
        private readonly IGroupService _groupService;

        public DashBoardController(IUserService userService,  DataDbContext dbContext, IGroupService groupService)
        {
            _userService = userService;
           
            _dbContext = dbContext;
            _groupService = groupService;
        }

        public IActionResult UpdateWorkingHour()
        {

            return View();

        }

        public IActionResult CheckHistory()
        {
            if (_groupService.IsInGroup() && !_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Intern"))
            {
                return View();
            }
            return View("AccessDenied");
        }



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
            else
            {
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

                ViewBag.AttendanceName = userAttendence.AttendanceName + "_" + _userService.GetFullName();
                if (userAttendence.userId != userId)
                {
                    //Error view 
                    //return View("Error");
                    return View("CustomNotFoundView");
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
