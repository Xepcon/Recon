using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.GroupLib;
using Recon.Utility;

namespace Recon.Controllers
{
    [Authenticated]
    [CustomRole("Hr", "Admin")]
    public class GroupsController : Controller
    {
        private readonly IGroupService _groupservice;
        private readonly IUserService _userService;
        public GroupsController(IGroupService groupservice, IUserService userService)
        {
            _userService = userService;
            _groupservice = groupservice;
        }

        // GET: Groups
        public IActionResult Index()
        {

            ViewBag.data = JsonConvert.SerializeObject(_groupservice.GetAllGroups());
            return View();
        }


        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }
        // GET: Groups/Edit
        public IActionResult Edit(int id)
        {


            var group = _groupservice.GetGroupById(id);
            if (group == null)
            {
                return View("CustomNotFoundView");
            }
            ViewBag.data = JsonConvert.SerializeObject(group);
            return View();
        }

        // Post: Groups/Edit
        [HttpPost]
        public IActionResult Edit(Group group)
        {
            ViewBag.ToastMessages = new List<ToastMessages>();
            if (group.groupId != null)
            {
                if (ModelState.IsValid)
                {
                    _groupservice.UpdateGroup(group);

                }

            }
            return RedirectToAction("Index");

        }
        // Post: Groups/Create
        [HttpPost]
        public IActionResult Create(Group group)
        {
            ViewBag.ToastMessages = new List<ToastMessages>();
            if (ModelState.IsValid)
            {
                _groupservice.CreateGroup(group);

                ViewBag.ToastMessages.Add(new ToastMessages
                {
                    message = $"Sikeres Létrehoztad a munkacsoportot <b>{group.Name}</b> néven ",
                    type = TypeToast.SUCCES,

                });

                return View();
            }
            ViewBag.ToastMessages.Add(new ToastMessages
            {
                message = $"Sikeretelen a munkacsoport léttrehozása  ",
                type = TypeToast.ERROR,

            });
            return View(group);
        }
        // Post: Groups/Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (_groupservice.GetGroupById(id) == null || !_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Admin" || r.Name == "Hr"))
            {
                return NotFound();
            }
            _groupservice.DeleteGroup(id);


            return RedirectToAction("Index");
        }


    }
}
