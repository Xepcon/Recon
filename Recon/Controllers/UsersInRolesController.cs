﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Models.Model.Account;
using Recon.Models.Repository;
using Recon.Utility;

namespace Recon.Controllers
{
    [Authenticated]
    [CustomRole("Admin", "Hr")]
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
                ViewBag.ToastMessages.Add(new ToastMessages
                {
                    message = "Sikertelen volt a felhasználó törlése",
                    type = TypeToast.ERROR,

                });
                return View();
                
            }

            var existingUsersInRoles = _usersInRolesRepository.GetById(roleId.Value, userId.Value);
            if (existingUsersInRoles == null)
            {
                ViewBag.ToastMessages.Add(new ToastMessages
                {
                    message = "Sikertelen volt a felhasználó törlése nem található a felhasználó",
                    type = TypeToast.ERROR,

                });
                return View();
            }

            _usersInRolesRepository.Delete(roleId.Value, userId.Value);

            return RedirectToAction("Index");
        }



    }

}
