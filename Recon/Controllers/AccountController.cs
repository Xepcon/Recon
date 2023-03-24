using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Data;
using Recon.Models;
using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;
using Recon.Utility;
using Recon.ViewModel;
using System.Diagnostics;
using System.Security.Claims;

namespace Recon.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        
        public AccountController(IUserService userService )
        {
            _userService = userService;            
        }

        [Authenticated]
        [HttpGet]
        public IActionResult UpdatePassword()
        {

            return View();

        }
        [Authenticated]
        [HttpPost]
        public IActionResult UpdatePassword(ChangePasswordViewModel model)
        {
            
            if (model != null)
            {
                int userId = _userService.GetUserId();
                var user = _userService.GetById(userId);


                if (userId != null)
                {

                    if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.PasswordHash))
                    {
                        ModelState.AddModelError("OldPassword", "Old password is incorrect");
                        return View(model);
                    }

                    _userService.ChangePassword(userId, model.NewPassword);
                    _userService.LogOut();

                    // redirect the user to the login page
                    return RedirectToAction("Login", "Account");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

       

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {           
            if (ModelState.IsValid)
            {
                 _userService.Register(model);                
                return RedirectToAction("Login");
                              
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {          
            return View();
        }

      
       

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.Authenticate(model.Username, model.Password);

                if (user != null)
                {                                         
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Helytelen felhasználónév vagy jelszó");
                    return View(model);
                }
            }

            return View(model);
        }
        [Authenticated]
        [HttpGet]
        public IActionResult UpdatePersonalInfo()
        {        
            var person = _userService.UserGetPersonalInfo(_userService.GetUserId());              
            ViewBag.data = JsonConvert.SerializeObject(person);
            if (person == null)
            {
                return View("Error");
            }
            return View();            
        }
        [Authenticated]
        [HttpPost]
        public IActionResult UpdatePersonalInfo(Person model)
        {                   
            int userId = _userService.GetUserId();
            model.userId = userId;
               
            if (ModelState.IsValid)
            {
                _userService.UserUpdatePersonalInfo(model);                    
                return RedirectToAction("Index","Dashboard");
            }
            return View(model);           
        }

        public IActionResult Logout()
        {
            _userService.LogOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
