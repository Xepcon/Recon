using Microsoft.AspNetCore.Mvc;
using Recon.Models;
using System.Diagnostics;
using Recon.Attribute;
using Recon.Models.Interface.Account;
using Recon.Utility;

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
            ViewBag.ToastMessages = new List<ToastMessages>();
            ToastMessages toast = new ToastMessages
            {
                message = "This is a test message",
                type = TypeToast.ERROR,
                time = 3000
            };
            ToastMessages toast2 = new ToastMessages
            {
                message = "This is a test message2",
                type = TypeToast.INFO,
                time = 3000
            };
            ViewBag.ToastMessages.Add(toast);
            ViewBag.ToastMessages.Add(toast2);
            return View();
        }

       
    }
}