using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recon.Data;
using Recon.Models;
using Recon.ViewModel;
using System.Diagnostics;
using System.Drawing.Printing;

namespace Recon.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataDbContext _dbContext;

        public DashBoardController(IUserService userService, IHttpContextAccessor httpContextAccessor, DataDbContext dbContext) {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UpdatePassword() { 

            return View();
            
        }

        [HttpPost]
        public IActionResult UpdatePassword(ChangePasswordViewModel model)
        {
            Debug.WriteLine("CALLED UPDATE PASS");


            Debug.WriteLine(model.ToString());
            if (model != null)
            {
                int userId = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("UserId"));
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
    }
}
