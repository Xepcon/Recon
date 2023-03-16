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
using Recon.Data;
using Recon.Models.Model.Account;
using Recon.Models.Model.Group;

namespace Recon.Controllers
{
    public class GroupMembersController : Controller
    {
        private readonly DataDbContext _dbContext;

        public GroupMembersController(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            if (_dbContext.GroupMembers.ToList() != null)
            {
                ViewBag.data = JsonConvert.SerializeObject(_dbContext.GroupMembers.ToList());

            }
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
                var list = _dbContext.GroupMembers.Where(x=>x.groupId== model.groupId && x.userId==model.userId).ToList();
                if (list.IsNullOrEmpty())
                {
                    _dbContext.Add(model);
                    _dbContext.SaveChanges();

                    //await _context.SaveChangesAsync();
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
            Debug.WriteLine(groupid);
            Debug.WriteLine(userid);
            if (groupid != null && userid != null)
            {
                try
                {
                    var model = _dbContext.GroupMembers.Where(x => x.groupId == groupid & x.userId == userid).FirstOrDefault();
                    if (model != null)
                    {
                        _dbContext.GroupMembers.Remove(model);
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        return NotFound();
                    }
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
