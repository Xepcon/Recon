using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Card;
using Recon.Models.Model.GroupLib;
using Recon.Utility;

namespace Recon.Controllers
{
    [Authenticated]
    public class MagneticCardController : Controller
    {
        private readonly IUserService _userService;
        private readonly DataDbContext _dbContext;

        public MagneticCardController(IUserService userService, DataDbContext dbContext)
        {
            _userService = userService;
            _dbContext = dbContext;
        }
        
        public IActionResult Index()
        {
            ViewBag.data = JsonConvert.SerializeObject(_dbContext.magneticCards);
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MagneticCard card)
        {
            ViewBag.ToastMessages = new List<ToastMessages>();
            if (ModelState.IsValid)
            {
                if (!_dbContext.magneticCards.Where(x => x.userId == card.userId).Any())
                {


                    _dbContext.magneticCards.Add(card);
                    _dbContext.SaveChanges();

                    ViewBag.ToastMessages.Add(new ToastMessages
                    {
                        message = $"Sikeres hozzárendelted a kártyát a felhasználóhoz ",
                        type = TypeToast.SUCCES,

                    });

                    return View();
                }
                else {
                    ViewBag.ToastMessages.Add(new ToastMessages
                    {
                        message = $"Sikertelen a kártya hozzárendelése a felhasználóhoz, már a felhasználóhóz van kártya rendelve ",
                        type = TypeToast.ERROR,

                    });
                    return View(card);
                }
            }
            ViewBag.ToastMessages.Add(new ToastMessages
            {
                message = $"Sikeretelen a kártya hozzárendelése a felhasználóhoz   ",
                type = TypeToast.ERROR,

            });
            return View(card);
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var cardAssosiation = _dbContext.magneticCards.Where(x => x.CardId == id).FirstOrDefault();
            if (cardAssosiation == null)
            {
                return NotFound();
            }
            
            if (cardAssosiation != null)
            {
                _dbContext.magneticCards.Remove(cardAssosiation);
                _dbContext.SaveChanges();
            }


            return RedirectToAction("Index");
        }
       
        }
    }
