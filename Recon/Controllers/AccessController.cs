using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Recon.Models;

namespace Recon.Controllers
{
    public class AccessController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(ViewModelLogin model)
        {
            if (ModelState.IsValid)
            {


            }

            return View();
        }
    }
}
