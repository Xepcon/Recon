using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Account;
using Recon.Models.Model.GroupLib;
using Recon.Utility;

namespace Recon.Controllers
{
    [Authenticated]
    public class GroupMembersController : Controller
    {
        private readonly IGroupService _groupservice;
        //private readonly IUserService _userservice; 

        public GroupMembersController(IGroupService groupservice)
        {
           _groupservice= groupservice;
            //_userservice= userService;
        }
        public IActionResult Index()
        {
            
            ViewBag.data = JsonConvert.SerializeObject(_groupservice.GetAllMembers());            
            return View();
        }


        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }
                      
        [HttpPost]
        public IActionResult Create(GroupMember model)
        {
            ViewBag.ToastMessages = new List<ToastMessages>();
            if (ModelState.IsValid)
            {
                bool succes = _groupservice.AddMembers(model);
                if(succes)
                {
                    ViewBag.ToastMessages.Add(new ToastMessages
                    {
                        message = "Sikeres hozzárendelted a felhasználót a munkacsoporthoz ",
                        type = TypeToast.SUCCES,

                    });
                    return View();
                }
                else {
                    ViewBag.ToastMessages.Add(new ToastMessages
                    {
                        message = "Nem sikerült hozzárendelni a felhasználót a munkacsoporthoz",
                        type = TypeToast.ERROR,

                    });
                    return View();
                }
            }
            return View(model);
        }
        
        [HttpPost]
        public IActionResult Delete(int groupid,int userid)
        {
            
            if (groupid != null && userid != null )
            {
                try
                {
                    _groupservice.DeleteMembers(groupid,userid);    
                    
                }
                catch (Exception e)
                {
                    return View("CustomNotFoundView");
                }
                return RedirectToAction("Index");
            }
            else {
                return View("CustomNotFoundView");
            }
          
           
        }
      
    }
}
