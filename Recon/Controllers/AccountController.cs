using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Models.Interface.Account;
using Recon.Models.Model.Account;
using Recon.Utility;
using Recon.ViewModel;

namespace Recon.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
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
            ViewBag.ToastMessages = new List<ToastMessages>();
            if (model != null)
            {
                int userId = _userService.GetUserId();
                var user = _userService.GetById(userId);


                if (userId != null)
                {

                    if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.PasswordHash))
                    {
                        //ModelState.AddModelError("OldPassword", "Old password is incorrect");
                        ViewBag.ToastMessages.Add(new ToastMessages
                        {
                            message = "Sikertelen volt a jelszó változtatás a jelenlegi jelszó helytelen",
                            type = TypeToast.ERROR,

                        });
                        return View();
                    }

                    _userService.ChangePassword(userId, model.NewPassword);

                    ViewBag.ToastMessages.Add(new ToastMessages
                    {
                        message = "Sikeresen megváltoztattad a jelszavad",
                        type = TypeToast.SUCCES,

                    });
                    //_userService.LogOut();
                    return View();
                    // redirect the user to the login page
                    //return RedirectToAction("Login", "Account");
                }
            }

            return View();
        }
        [Authenticated]
        [HttpGet]
        [CustomRole( "Admin")]
        public IActionResult Register()
        {
            return View();
        }


        [Authenticated]
        [HttpPost]
        [CustomRole("Admin")]
        public IActionResult Register(RegisterViewModel model)
        {
            ViewBag.ToastMessages = new List<ToastMessages>();
            if (ModelState.IsValid)
            {
                _userService.Register(model);
                ViewBag.ToastMessages.Add(new ToastMessages
                {
                    message = $"Sikeres létrehoztál egy új fiókot ",
                    type = TypeToast.SUCCES,

                });
                return View(model);
                //return RedirectToAction("Login");

            }
            ViewBag.ToastMessages.Add(new ToastMessages
            {
                message = $"Sikertelen  volt a regisztrálás ",
                type = TypeToast.ERROR,

            });
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.AuthenticateAsync(model.Username, model.Password);

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

        /*[HttpPost]
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
        }*/
        [Authenticated]
        [HttpGet]
        public IActionResult UpdatePersonalInfo()
        {

            var person = _userService.UserGetPersonalInfo(_userService.GetUserId());
            ViewBag.data = JsonConvert.SerializeObject(person);
            if (person == null)
            {

                return View();
            }

            return View();
        }
        [Authenticated]
        [HttpPost]
        public IActionResult UpdatePersonalInfo(Person model)
        {
            ViewBag.ToastMessages = new List<ToastMessages>();
            int userId = _userService.GetUserId();
            model.userId = userId;


            if (ModelState.IsValid)
            {
                _userService.UserUpdatePersonalInfo(model);
                ViewBag.ToastMessages.Add(new ToastMessages
                {
                    message = $"Sikeres módosítottad a személyes adataitad ",
                    type = TypeToast.SUCCES,

                });
                var person = _userService.UserGetPersonalInfo(_userService.GetUserId());
                ViewBag.data = JsonConvert.SerializeObject(person);
                return View(model);
            }
            ViewBag.ToastMessages.Add(new ToastMessages
            {
                message = "Sikeretelen volt a módosítás ",
                type = TypeToast.ERROR,

            });
            return View(model);
        }

        public IActionResult Logout()
        {
            _userService.LogOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
