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
using NuGet.Packaging;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.Group;
using Recon.Models.Model.Group;
using Recon.Models.Model.Ticket;
using Recon.Models.Model.TimeManager;

namespace Recon.Controllers
{
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
                    if (_groupService.isInGroup()) {
                        int numOfGroups = _dbContext.GroupMembers.Where(x => x.userId == attendance.userId).Count();
                        Debug.WriteLine(numOfGroups);
                        if (numOfGroups == 1) {
                            attendance.groupId = _dbContext.GroupMembers.Where(x => x.userId == attendance.userId).FirstOrDefault().groupId;                            
                        }
                        else
                        {
                            return View("Error");
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
                    _dbContext.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else {
                    return BadRequest("No user");
                }
            }
            return View(attendance);
        }

        public IActionResult ApproveAttendance() {
            if (_userService.IsAuthenticated()) 
            {
                if (_groupService.isGroupOwner()) 
                {
                    
                    IEnumerable<IGroup> userGroupsPrincipal = _groupService.getUserGroup().Where(x => x.principalId == _userService.getUserId());

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
                return View("Error");
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]

        public IActionResult ApproveAtt(int id)
        {
            if (id != null)
            {
                try
                {
                    var model = _dbContext.Attendances.Where(x => x.AttendanceId == id).FirstOrDefault();
                    if(!_groupService.isInGroup(_userService.getUserId(),model.groupId) || !_groupService.isGroupOwner()){
                        return NotFound();
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
                        return NotFound();
                    }
                }
                catch (Exception e)
                {
                    return NotFound();
                }
                return RedirectToAction("ApproveAttendance");
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult ApproveSheetAttendance(int id) {
            if (_userService.IsAuthenticated())
            {
                ViewBag.AttendanceId = id;              
           
                return View();

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
