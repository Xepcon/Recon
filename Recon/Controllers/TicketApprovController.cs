﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Data;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Ticket;
using Recon.Models.Repository;
using System.Diagnostics;

namespace Recon.Controllers
{
    [Authenticated]
    public class TicketApprovController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly ITicketRepository _ticketRepository;
    

        public TicketApprovController(IUserService userService, IGroupService groupService, ITicketRepository ticketRepository)
        {
            _userService = userService;
            _groupService = groupService;
           
            _ticketRepository = ticketRepository;
        }
       
        public IActionResult Index()
        {
         
            if (_groupService.IsGroupOwner())
            {
                IEnumerable<IGroup> userGroupsPrincipal = _groupService.getUserGroup().Where(x => x.principalId == _userService.GetUserId());

                List<DayOffTicket> res = _ticketRepository.GetAllTicketsForPrincipal(userGroupsPrincipal);
               
               
                return View(res);
            }
            else
            {
                return RedirectToAction("Index", "DashBoard");
            }           

        }
        [HttpPost]
        
        public IActionResult Approve(int id)
        {           
            if (_ticketRepository.ApproveTicket(id))
            {
                return RedirectToAction("Index", "TicketApprov");
            } 
            //Hiba 
            return RedirectToAction("Index", "DashBoard");
          
        }
        [Authenticated]
        public IActionResult TicketHistory()
        {

            var dataWithTickets = _ticketRepository.GetUsersTicket(_userService.GetUserId());
           
            return View(dataWithTickets);
           
           
        }

        public IActionResult DayOff()
        {

            ViewBag.UserGroup = JsonConvert.SerializeObject(_groupService.getUserGroup());
            return View();
        }
        [Authenticated]
        [HttpPost]
        public IActionResult DayOff(DayOffTicket model)
        {
            model.userId = _userService.GetUserId();
            _ticketRepository.CreateTicket(model);
            return RedirectToAction("Index","Dashboard");
           
        }

    }
}
