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
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Model.Account;
using Recon.Models.Repository;

namespace Recon.Controllers
{
    [Authenticated]
    public class RolesController : Controller
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesController(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }
        public IActionResult Index()
        {
            ViewBag.data = JsonConvert.SerializeObject(_rolesRepository.GetAllRoles());
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(int id)
        {
            if ( _rolesRepository.GetRoleById(id) == null)
            {
                return NotFound();
            }

            var model = _rolesRepository.GetRoleById(id);
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
         
            if (ModelState.IsValid)
            {
                _rolesRepository.UpdateRole(role);                                        
            }           
            return RedirectToAction("Index");

        }
        [HttpPost]
        public IActionResult Create(Roles role)
        {
            
            if (ModelState.IsValid)
            {
                _rolesRepository.AddRole(role);
                              
                return RedirectToAction("Index");
            }
            return View(role);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {

            if (_rolesRepository.GetRoleById(id) == null)
            {
                return NotFound();
            }

            var model = _rolesRepository.GetRoleById(id);
            if (model == null)
            {
                return NotFound();
            }

            _rolesRepository.DeleteRole(id);

            return RedirectToAction("Index");
        }

    }
}
