using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Recon.Data;
using Recon.Models.Model.Account;

namespace Recon.Controllers
{
    public class RolesController : Controller
    {
        private readonly DataDbContext _dbContext;

        public RolesController(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            if (_dbContext.Role.ToList() != null)
            {
                ViewBag.data = JsonConvert.SerializeObject(_dbContext.Role.ToList());

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
            if (id == null || _dbContext.Role == null)
            {
                return NotFound();
            }

            var model = _dbContext.Role.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewBag.data = JsonConvert.SerializeObject(model);
            return View();
        }
        [HttpPost]

        public IActionResult Edit(Roles role)
        {

            if (role.Id != null)
            {
                if (ModelState.IsValid)
                {
                    _dbContext.Update(role);
                    _dbContext.SaveChanges();
                }
            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        public IActionResult Create(Roles role)
        {
            
            if (ModelState.IsValid)
            {
                _dbContext.Add(role);
                _dbContext.SaveChanges();
                //await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {

            if (id == null || _dbContext.Role == null)
            {
                return NotFound();
            }

            var model = _dbContext.Role.Find(id);
            if (model == null)
            {
                return NotFound();
            }

            _dbContext.Role.Remove(model);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
