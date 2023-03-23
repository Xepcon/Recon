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

namespace Recon.Controllers
{
    [Authenticated]
    public class GroupMembersController : Controller
    {
        private readonly IGroupService _groupservice;
      

        public GroupMembersController(IGroupService groupservice)
        {
           _groupservice= groupservice;
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
           
            if (ModelState.IsValid)
            {
                bool succes = _groupservice.AddMembers(model);
                if(succes)
                {
                    return RedirectToAction("Index");
                }
                else {
                    return NotFound();
                }
            }
            return View(model);
        }
        
        [HttpPost]
        public IActionResult Delete(int groupid,int userid)
        {
           
            if (groupid != null && userid != null)
            {
                try
                {
                    _groupservice.DeleteMembers(groupid,userid);    
                    
                }
                catch (Exception e)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
            }
            else {
                return NotFound();
            }
          
           
        }
      
    }
}
