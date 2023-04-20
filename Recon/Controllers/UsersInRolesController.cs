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
using Recon.Utility;

namespace Recon.Controllers
{
    [Authenticated]
    [CustomRole("Admin","Hr")]
    public class UsersInRolesController : Controller
    {
        
        private readonly IUsersInRolesRepository _usersInRolesRepository;
        public UsersInRolesController(IUsersInRolesRepository usersInRolesRepository)
        {
            _usersInRolesRepository = usersInRolesRepository;
          
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

       /* public IActionResult Edit(int? roleId, int? userId)
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
        }*/

        [HttpPost]
        public IActionResult Create(UsersInRoles usersInRoles)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ToastMessages = new List<ToastMessages>();
                var existingUsersInRoles = _usersInRolesRepository.GetById(usersInRoles.roleId, usersInRoles.userId);
                if (existingUsersInRoles != null)
                {
                    ViewBag.ToastMessages.Add(new ToastMessages
                    {
                        message = "Sikertelen volt a hozzárendelés a felhasználó már a szerepkörhöz tartozik",
                        type = TypeToast.ERROR,

                    });
                    return View();
                }

                _usersInRolesRepository.Add(usersInRoles);
                ViewBag.ToastMessages.Add(new ToastMessages
                {
                    message = "Sikeresen hozzárendelted a felhasználót a szerepkörhöz",
                    type = TypeToast.SUCCES,

                });

                //return RedirectToAction("Index");
                return View();
            }

            return View();
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


        
    }
    
}
