using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recon.Data;
using Recon.Models.Model.Account;

namespace Recon.Controllers
{
    public class UsersInRolesController : Controller
    {
        private readonly DataDbContext _context;

        public UsersInRolesController(DataDbContext context)
        {
            _context = context;
        }

        // GET: UsersInRoles
        public async Task<IActionResult> Index()
        {
              return _context.UsersInRole != null ? 
                          View(await _context.UsersInRole.ToListAsync()) :
                          Problem("Entity set 'DataDbContext.UsersInRole'  is null.");
        }

        // GET: UsersInRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UsersInRole == null)
            {
                return NotFound();
            }

            var usersInRoles = await _context.UsersInRole
                .FirstOrDefaultAsync(m => m.roleId == id);
            if (usersInRoles == null)
            {
                return NotFound();
            }

            return View(usersInRoles);
        }

        // GET: UsersInRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UsersInRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("userId,roleId")] UsersInRoles usersInRoles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usersInRoles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usersInRoles);
        }

        // GET: UsersInRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UsersInRole == null)
            {
                return NotFound();
            }

            var usersInRoles = await _context.UsersInRole.FindAsync(id);
            if (usersInRoles == null)
            {
                return NotFound();
            }
            return View(usersInRoles);
        }

        // POST: UsersInRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("userId,roleId")] UsersInRoles usersInRoles)
        {
            if (id != usersInRoles.roleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersInRoles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersInRolesExists(usersInRoles.roleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usersInRoles);
        }

        // GET: UsersInRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UsersInRole == null)
            {
                return NotFound();
            }

            var usersInRoles = await _context.UsersInRole
                .FirstOrDefaultAsync(m => m.roleId == id);
            if (usersInRoles == null)
            {
                return NotFound();
            }

            return View(usersInRoles);
        }

        // POST: UsersInRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UsersInRole == null)
            {
                return Problem("Entity set 'DataDbContext.UsersInRole'  is null.");
            }
            var usersInRoles = await _context.UsersInRole.FindAsync(id);
            if (usersInRoles != null)
            {
                _context.UsersInRole.Remove(usersInRoles);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersInRolesExists(int id)
        {
          return (_context.UsersInRole?.Any(e => e.roleId == id)).GetValueOrDefault();
        }
    }
}
