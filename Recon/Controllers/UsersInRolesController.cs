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
using Recon.Data;
using Recon.Models.Model.Account;

namespace Recon.Controllers
{
    public class UsersInRolesController : Controller
    {
        private readonly DataDbContext _dbContext;

        public UsersInRolesController(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
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
            Debug.WriteLine(model.roleId);
            Debug.WriteLine(model.userId);
            Debug.WriteLine("ANADD");
             if (model.roleId != null && model.userId!=null)
             {
                Debug.WriteLine("Good1");
                if (ModelState.IsValid)
                 {
                    Debug.WriteLine("Good2");
                    var list = _dbContext.UsersInRole.Where(x => x.roleId == model.roleId && x.userId == model.userId);
                    if (list.IsNullOrEmpty())
                    {
                        Debug.WriteLine("Good3");
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
         }
     }
    
}
