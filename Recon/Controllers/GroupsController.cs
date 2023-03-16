using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Recon.Data;
using Recon.Models.Model.Account;
using Recon.Models.Model.Group;

namespace Recon.Controllers
{
    public class GroupsController : Controller
    {
        private readonly DataDbContext _dbContext;

        public GroupsController(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Groups
        public IActionResult Index()
        {
            if (_dbContext.Groups.ToList() != null)
            {
                ViewBag.data = JsonConvert.SerializeObject(_dbContext.Groups.ToList());

            }
            return View();
        }

       
        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || _dbContext.Groups == null)
            {
                return NotFound();
            }

            var group = _dbContext.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
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
                    _dbContext.Update(group);
                    _dbContext.SaveChanges();
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
                _dbContext.Add(group);
                _dbContext.SaveChanges();
                //await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            
            if (id == null || _dbContext.Groups == null)
            {
                return NotFound();
            }

            var group = _dbContext.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            _dbContext.Groups.Remove(group);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
