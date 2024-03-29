﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recon.Attribute;
using Recon.Models.Interface.Account;
using Recon.Models.Interface.GroupLib;
using Recon.Models.Model.Ticket;
using Recon.Models.Repository;
using Recon.Utility;

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
            if (!_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Intern"))
            {
                if (_groupService.IsGroupOwner())
                {
                    IEnumerable<IGroup> userGroupsPrincipal = _groupService.getUserGroup().Where(x => x.principalId == _userService.GetUserId());

                    List<DayOffTicket> res = _ticketRepository.GetAllTicketsForPrincipal(userGroupsPrincipal).Where(ticket => ticket.isApproved == false)
                                     .ToList(); ;


                    return View(res);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("AccessDenied");
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
        public IActionResult TicketHistory(int? page)
        {
            if (_groupService.IsInGroup() && !_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Intern"))
            {
                var dataWithTickets = _ticketRepository.GetUsersTicket(_userService.GetUserId());
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                var pagedData = dataWithTickets.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                int totalCount = dataWithTickets.Count();
                int pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

                ViewBag.PageCount = pageCount;
                ViewBag.TotalItemCount = totalCount;

                return View(pagedData);
            }
            return View("AccessDenied");
        }

        public IActionResult DayOff()
        {
            if (_groupService.IsInGroup() && !_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Intern"))
            {
                ViewBag.UserGroup = JsonConvert.SerializeObject(_groupService.getUserGroup());
                return View();
            }
            return View("AccessDenied");

        }

        [HttpPost]
        public IActionResult DayOff(DayOffTicket model)
        {
            if (_groupService.IsInGroup() && !_userService.GetRolesForUser(_userService.GetUserId()).Any(r => r.Name == "Intern"))
            {
                ViewBag.ToastMessages = new List<ToastMessages>();
                ViewBag.UserGroup = JsonConvert.SerializeObject(_groupService.getUserGroup());
                model.userId = _userService.GetUserId();
                model.Created = DateTime.Now;
                if (ModelState.IsValid)
                {
                    _ticketRepository.CreateTicket(model);
                    ViewBag.ToastMessages.Add(new ToastMessages
                    {
                        message = "Sikeresen létrehoztad a szabadsági kérelmedet",
                        type = TypeToast.SUCCES,

                    });
                    return View("DayOff", ViewBag);
                }
                ViewBag.ToastMessages.Add(new ToastMessages
                {
                    message = "Sikeretelen volt a szabadsági kérelmed létrehozása",
                    type = TypeToast.ERROR,

                });
                return View("DayOff");
            }
            return View("AccessDenied");

        }

    }
}
