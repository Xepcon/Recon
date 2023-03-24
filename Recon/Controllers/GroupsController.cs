using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Account;
using Recon.Models.Model.GroupLib;

namespace Recon.Controllers
{
    [Authenticated]
    public class GroupsController : Controller
    {
        private readonly IGroupService _groupservice;

        public GroupsController(IGroupService groupservice)
        {
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
        public IActionResult Edit(int id)
        {
            if (_groupservice.GetGroupById(id)==null)
            {
                return View("CustomNotFoundView");
            }

            var group = _groupservice.GetGroupById(id);
            if (group == null)
            {
                return View("CustomNotFoundView");
            }
            ViewBag.data = JsonConvert.SerializeObject(group);
            return View();
        }

        [HttpPost]
   
        public IActionResult Edit(Group group)
        {
          
            if (group.groupId != null) {
                if (ModelState.IsValid)
                {
                    _groupservice.UpdateGroup(group);
                }
            }
            return RedirectToAction("Index");
           
        }
        [HttpPost]
        public IActionResult Create( Group group)
        {
            Debug.WriteLine(group.Name);
            if (ModelState.IsValid)
            {
                _groupservice.CreateGroup(group);

                return RedirectToAction("Index");
            }
            return View(group);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            
            if (_groupservice.GetGroupById(id) == null)
            {
                return NotFound();
            }
            _groupservice.DeleteGroup(id);
                 

            return RedirectToAction("Index");
        }


    }
}
