using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recon.Data;
using Recon.Models;
using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;
using Recon.ViewModel;
using System.Diagnostics;
using System.Security.Claims;

namespace Recon.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataDbContext _dbContext;

        public AccountController(IUserService userService, IHttpContextAccessor httpContextAccessor, DataDbContext dbContext)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

       

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            Debug.Write("CALLED");
            if (ModelState.IsValid)
            {
                Debug.Write("GOOD");
                var user = new UserEntity
                {
                    Username = model.Username,
                    PasswordHash = model.Password,
                    Email = model.Email,                                    
                };
              
                try
                {
                    var Tempororary = new Person();
                    Tempororary.userId = user.Id;
                    _dbContext.Person.Add(Tempororary);
                    _dbContext.SaveChanges();
                    _userService.Create(user);
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while registering the user.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
          
            return View();
        }

      
        public IActionResult Logout()
        {
            _userService.LogOut();
            return RedirectToAction("Index", "Home");
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
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }
            }

            return View(model);
        }


    }
}
