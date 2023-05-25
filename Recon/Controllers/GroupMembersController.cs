using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Account;
using Recon.Models.Model.GroupLib;
using Recon.Utility;
using System.Diagnostics;

namespace Recon.Controllers
{
    [Authenticated]
    
    public class GroupMembersController : Controller
    {
        private readonly IGroupService _groupservice;
        private readonly IUserService _userService; 

        public GroupMembersController(IGroupService groupservice,IUserService userService)
        {
            _groupservice = groupservice;
            _userService= userService;
        }
        public IActionResult Index()
        {
            // Debug.WriteLine( _userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Admin"));
            //_userservice.GetRolesForUser(_userservice.GetUserId())
   

            if(_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Intern"))
                return View("AccessDenied");
            
            if (//_groupservice.getUserGroup().Count() > 0
                _groupservice.IsGroupOwner()
                || _groupservice.IsGroupOwnerAndMember()
                || _userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Admin"))
            {
                ViewBag.data = JsonConvert.SerializeObject(_groupservice.GetAllMembers());
                return View();
            }
            return View("AccessDenied");
        }


        // GET: Groups/Create
        public IActionResult Create()
        {

            if (_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Intern"))
                return View("AccessDenied");

            if (_groupservice.IsGroupOwner() ||
                _groupservice.IsGroupOwnerAndMember() || _userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Admin"))
            {
                return View();
            }
            return View("AccessDenied");
        }

        [HttpPost]
        public IActionResult Create(GroupMember model)
        {
            if (_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Intern"))
                return View("AccessDenied");

            if (_groupservice.IsGroupOwner() ||  _groupservice.IsGroupOwnerAndMember() || _userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Admin"))
            {
                ViewBag.ToastMessages = new List<ToastMessages>();
                if (ModelState.IsValid)
                {
                    bool succes = _groupservice.AddMembers(model);
                    if (succes)
                    {
                        ViewBag.ToastMessages.Add(new ToastMessages
                        {
                            message = "Sikeres hozzárendelted a felhasználót a munkacsoporthoz ",
                            type = TypeToast.SUCCES,

                        });
                        return View();
                    }
                    else
                    {
                        ViewBag.ToastMessages.Add(new ToastMessages
                        {
                            message = "Nem sikerült hozzárendelni a felhasználót a munkacsoporthoz",
                            type = TypeToast.ERROR,

                        });
                        return View();
                    }
                }
                return View(model);
            }
            return View("AccessDenied");
        }

        [HttpPost]
        public IActionResult Delete(int groupid, int userid)
        {
            if (_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Intern"))
                return View("AccessDenied");

            if (_groupservice.IsGroupOwner()  || _groupservice.IsGroupOwnerAndMember() || _userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Admin"))
            {
                if (groupid != null && userid != null)
                {
                    try
                    {
                        _groupservice.DeleteMembers(groupid, userid);

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

    }
}
