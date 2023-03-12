using Microsoft.AspNetCore.Mvc;
using Recon.Models;
using System.Diagnostics;
using Recon.Attribute;
using Recon.Models.Interface.Account;

namespace Recon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //private readonly IUserService _userService;

        //IUserService userService
        public HomeController(ILogger<HomeController> logger )
        {
            _logger = logger;
           // _userService = userService;

        }

        public IActionResult Index()
        {
            return View();
        }

        [Authenticated]
        [CustomRole("Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authenticated]
        [CustomRole("Intern")]

        public IActionResult Hr()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}