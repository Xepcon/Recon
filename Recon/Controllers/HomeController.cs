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

       
    }
}