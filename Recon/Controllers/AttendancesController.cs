using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NuGet.Packaging;
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.GroupLib;
using Recon.Models.Model.Ticket;
using Recon.Models.Model.TimeManager;
using Recon.Utility;

namespace Recon.Controllers
{
    [Authenticated]
    public class AttendancesController : Controller
    {    
        private readonly DataDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        public AttendancesController(DataDbContext dbContext, IUserService userService, IGroupService groupService)
        {
            _userService = userService;
            _dbContext = dbContext;
            _groupService = groupService;
        }
        public IActionResult Index()
        {
            if (_groupService.IsGroupOwner() || _userService.IsInRole("Admin") || _userService.IsInRole("Hr"))
            {
                if (_dbContext.Attendances.ToList() != null)
                {
                    ViewBag.data = JsonConvert.SerializeObject(_dbContext.Attendances.ToList());

                }

                return View();
            }
            return View("AccessDenied");
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (_groupService.IsGroupOwner() || _userService.IsInRole("Admin") || _userService.IsInRole("Hr"))
            {
                ViewBag.ToastMessages = new List<ToastMessages>();
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
                     
                            return View("CustomNotFoundView");
                        }
                    }
                    catch (Exception e)
                    {
                        return View("CustomNotFoundView");
                    }
               
               
                
                    return RedirectToAction("Index");
                }
                else
                {


                    return View("CustomNotFoundView");
                }
            }
            return View("AccessDenied");

        }
        // GET: Attendances/Create
        public IActionResult Create()
        {
            if (_groupService.IsGroupOwner() || _userService.IsInRole("Admin") || _userService.IsInRole("Hr"))
            {
                return View();
            }
            return View("AccessDenied");
        }
       
        [HttpPost]
        
        public IActionResult Create( Attendance attendance)
        {
            if (_groupService.IsGroupOwner() || _userService.IsInRole("Admin") || _userService.IsInRole("Hr"))
            {
                ViewBag.ToastMessages = new List<ToastMessages>();
            
                if (ModelState.IsValid)
                {

                    var user = _dbContext.Person.Where(x => x.userId == attendance.userId).FirstOrDefault();
                    if (user != null)
                    {

                        if (_userService.GetRolesForUser(attendance.userId).Any(r => r.Name == "Intern"))
                        {
                            if (_groupService.IsInGroup() || _userService.IsInRole("Admin"))
                            {
                                int numOfGroups = _dbContext.GroupMembers.Where(x => x.userId == attendance.userId).Count();
                                Debug.WriteLine(numOfGroups);
                                if (numOfGroups == 1)
                                {
                                    attendance.groupId = _dbContext.GroupMembers.Where(x => x.userId == attendance.userId).FirstOrDefault().groupId;
                                }
                                else
                                {
                               
                                    ViewBag.ToastMessages.Add(new ToastMessages
                                    {
                                        message = "Hiba történt a jelenlétív léttrehozásánál, a felhasználó több munkacsoportba is tartozik",
                                        type = TypeToast.ERROR,

                                    });
                                    return View();
                                }
                            }
                           
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
                       
                            ViewBag.ToastMessages.Add(new ToastMessages
                            {
                                message = "Sikeresen léttrehoztad a jelenlétiívet",
                                type = TypeToast.SUCCES,

                            });
                            _dbContext.SaveChanges();
                            return View();
                        }
                        else {
                        
                            ViewBag.ToastMessages.Add(new ToastMessages
                            {
                                message = "Hiba történt a jelenlétív léttrehozásánál, a felhasználó nem diák",
                                type = TypeToast.ERROR,

                            });
                            return View();
                        }
                    }
                    else {
                    
                        ViewBag.ToastMessages.Add(new ToastMessages
                        {
                            message = "A felhasználó nem található",
                            type = TypeToast.ERROR,

                        });
                        return View();
                    }
                }
           
                ViewBag.ToastMessages.Add(new ToastMessages
                {
                    message = "Hibás adatokat adtál meg ",
                    type = TypeToast.INFO,

                });
          
                return View(attendance);
            }
            return View("AccessDenied");
        }

        public IActionResult ApproveAttendance() {
            if (_groupService.IsGroupOwner() || _userService.IsInRole("Admin") || _userService.IsInRole("Hr"))
            { 
                IEnumerable<IGroup> userGroupsPrincipal = _groupService.getUserGroup().Where(x => x.principalId == _userService.GetUserId());

                List<Attendance> res = new List<Attendance>();

                foreach (var item in userGroupsPrincipal)
                {
                    if (_dbContext.Attendances.Where(x => x.groupId == item.groupId).Any())
                    {

                        res.AddRange(_dbContext.Attendances.Where(x => x.groupId == item.groupId && x.isClosed == false).ToList());
                    }
                }
                ViewBag.data= JsonConvert.SerializeObject(res);
                Debug.WriteLine(res.Count);
                return View();
                
            }
            return View("AccessDenied");

        }

        [HttpPost]

        public IActionResult ApproveAtt(int id)
        {
            if (_groupService.IsGroupOwner() || _userService.IsInRole("Admin") || _userService.IsInRole("Hr"))
            {
                if (id != null)
                {
                    try
                    {
                        var model = _dbContext.Attendances.Where(x => x.AttendanceId == id).FirstOrDefault();
                        if (!_groupService.IsInGroup(_userService.GetUserId(), model.groupId) || !_groupService.IsGroupOwner())
                        {
                            return View("CustomNotFoundView");
                        }
                        if (model != null)
                        {
                            model.isClosed = true;
                            var selectedmodel = _dbContext.AttendanceEntitys.Where(x => x.AttendanceId == id).ToList();
                            foreach (var item in selectedmodel)
                            {
                                item.approved = true;
                            }
                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            return View("CustomNotFoundView");
                        }
                    }
                    catch (Exception e)
                    {
                        return View("CustomNotFoundView");
                    }
                    return RedirectToAction("ApproveAttendance");
                }
                else
                {
                    return View("CustomNotFoundView");
                }
            }
            return View("AccessDenied");
        }

        public IActionResult ApproveSheetAttendance(int id) {
            if (_groupService.IsGroupOwner() || _userService.IsInRole("Admin") || _userService.IsInRole("Hr"))
            {
                var model = _dbContext.Attendances.Where(x => x.AttendanceId == id).FirstOrDefault();
                if (model != null)
                {
                    ViewBag.AttendanceId = id;

                    return View();
                }
                return View("CustomNotFoundView");
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
            
            ViewBag.AttendanceId = id;

            int userId = _userService.GetUserId();
            int attendId = int.Parse(id);
            var userAttendence = _dbContext.Attendances.Where(x => x.AttendanceId == attendId).FirstOrDefault();
            if (userAttendence.isClosed == true)
                ViewBag.IsClosed = "false";
            else
                ViewBag.IsClosed = "true";

            ViewBag.AttendanceName = userAttendence.AttendanceName + "_" + _userService.GetUserName();
            if (userAttendence.userId != userId)
            {
                //Error view 
                return View("Error");
            }

            return View();

            
            

        }
        public IActionResult Riport()
        {
            if (_groupService.IsGroupOwner() || _userService.IsInRole("Admin") || _userService.IsInRole("Hr"))
            {
                return View();
            }
            return View("AccessDenied");
        }
    }
}
