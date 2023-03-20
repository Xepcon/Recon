using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Model.Account;
using Recon.Models.Repository;

namespace Recon.Controllers
{
    [Authenticated]
    public class UsersInRolesController : Controller
    {
        //private readonly DataDbContext _dbContext;
        private readonly IUsersInRolesRepository _usersInRolesRepository;
        public UsersInRolesController(IUsersInRolesRepository usersInRolesRepository)
        {
            _usersInRolesRepository = usersInRolesRepository;
            //_dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var usersInRoles = _usersInRolesRepository.GetAll();
            ViewBag.data = JsonConvert.SerializeObject(usersInRoles);
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int? roleId, int? userId)
        {
            if (roleId == null || userId == null)
            {
                return View("Error");
            }

            var usersInRoles = _usersInRolesRepository.GetById(roleId.Value, userId.Value);
            if (usersInRoles == null)
            {
                return View("Error");
            }

            ViewBag.data = JsonConvert.SerializeObject(usersInRoles);
            return View();
        }

        [HttpPost]
        public IActionResult Edit(UsersInRoles usersInRoles)
        {
            if (ModelState.IsValid)
            {
                var existingUsersInRoles = _usersInRolesRepository.GetById(usersInRoles.roleId, usersInRoles.userId);
                if (existingUsersInRoles == null)
                {
                    return View("Error");
                }

                existingUsersInRoles.roleId = usersInRoles.roleId;
                existingUsersInRoles.userId = usersInRoles.userId;

                _usersInRolesRepository.Update(existingUsersInRoles);

                return RedirectToAction("Index");
            }

            return View(usersInRoles);
        }

        [HttpPost]
        public IActionResult Create(UsersInRoles usersInRoles)
        {
            if (ModelState.IsValid)
            {
                var existingUsersInRoles = _usersInRolesRepository.GetById(usersInRoles.roleId, usersInRoles.userId);
                if (existingUsersInRoles != null)
                {
                    return View("Error");
                }

                _usersInRolesRepository.Add(usersInRoles);

                return RedirectToAction("Index");
            }

            return View(usersInRoles);
        }

        [HttpPost]
        public IActionResult Delete(int? roleId, int? userId)
        {
            if (roleId == null || userId == null)
            {
                return View("Error");
            }

            var existingUsersInRoles = _usersInRolesRepository.GetById(roleId.Value, userId.Value);
            if (existingUsersInRoles == null)
            {
                return View("Error");
            }

            _usersInRolesRepository.Delete(roleId.Value, userId.Value);

            return RedirectToAction("Index");
        }


        /*
        public IActionResult Index()
        {
            if (_dbContext.UsersInRole.ToList() != null)
            {
                ViewBag.data = JsonConvert.SerializeObject(_dbContext.UsersInRole.ToList());

            }
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(int? roleid, int? userid)
         {
            if (roleid == null && userid == null)
            {
                return View("Error");
             }

             var model = _dbContext.UsersInRole.Find(roleid, userid);
             if (model == null)
             {
                 return View("Error");
             }
             ViewBag.data = JsonConvert.SerializeObject(model);
             return View();
         }

         [HttpPost]

         public IActionResult Edit(UsersInRoles model)
         {

             if (model.roleId != null && model.userId!=null)
             {
               
                if (ModelState.IsValid)
                 {
                    
                    var list = _dbContext.UsersInRole.Where(x => x.roleId == model.roleId && x.userId == model.userId);
                    if (list.IsNullOrEmpty())
                    {
                       
                        _dbContext.UsersInRole.Update(model);
                        _dbContext.SaveChanges();
                    }
                    else {
                        return View("Error");
                    }
                     
                 }
             }
             return RedirectToAction("Index");

         }
         [HttpPost]
         public IActionResult Create(UsersInRoles model)
         {

             if (ModelState.IsValid)
             {
                 var list = _dbContext.UsersInRole.Where(x=>x.roleId == model.roleId && x.userId == model.userId);
                if (list.IsNullOrEmpty())
                {
                    _dbContext.UsersInRole.Add(model);
                    _dbContext.SaveChanges();
                    
                    return RedirectToAction("Index");
                }
                else {
                    return View("Error");
                }
                
             }
             return View(model);
         }

         [HttpPost]
         public IActionResult Delete(int? roleid, int? userid)
         {

             if (roleid == null || userid == null)
             {
                 return View("Error");
             }

             var model = _dbContext.UsersInRole.Find(roleid,userid);
             if (model == null)
             {
                 return View("Error");
             }

             _dbContext.UsersInRole.Remove(model);
             _dbContext.SaveChanges();

             return RedirectToAction("Index");
         }*/
    }
    
}
