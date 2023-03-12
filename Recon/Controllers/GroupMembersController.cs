using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recon.Data;
using Recon.Models.Model.Group;

namespace Recon.Controllers
{
    public class GroupMembersController : Controller
    {
        private readonly DataDbContext _context;

        public GroupMembersController(DataDbContext context)
        {
            _context = context;
        }

        // GET: GroupMembers
        public async Task<IActionResult> Index()
        {
              return _context.GroupMembers != null ? 
                          View(await _context.GroupMembers.ToListAsync()) :
                          Problem("Entity set 'DataDbContext.GroupMembers'  is null.");
        }

        // GET: GroupMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GroupMembers == null)
            {
                return NotFound();
            }

            var groupMember = await _context.GroupMembers
                .FirstOrDefaultAsync(m => m.groupId == id);
            if (groupMember == null)
            {
                return NotFound();
            }

            return View(groupMember);
        }

        // GET: GroupMembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GroupMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("groupId,userId")] GroupMember groupMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupMember);
        }

        // GET: GroupMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GroupMembers == null)
            {
                return NotFound();
            }

            var groupMember = await _context.GroupMembers.FindAsync(id);
            if (groupMember == null)
            {
                return NotFound();
            }
            return View(groupMember);
        }

        // POST: GroupMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("groupId,userId")] GroupMember groupMember)
        {
            if (id != groupMember.groupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupMemberExists(groupMember.groupId))
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
            return View(groupMember);
        }

        // GET: GroupMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GroupMembers == null)
            {
                return NotFound();
            }

            var groupMember = await _context.GroupMembers
                .FirstOrDefaultAsync(m => m.groupId == id);
            if (groupMember == null)
            {
                return NotFound();
            }

            return View(groupMember);
        }

        // POST: GroupMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GroupMembers == null)
            {
                return Problem("Entity set 'DataDbContext.GroupMembers'  is null.");
            }
            var groupMember = await _context.GroupMembers.FindAsync(id);
            if (groupMember != null)
            {
                _context.GroupMembers.Remove(groupMember);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupMemberExists(int id)
        {
          return (_context.GroupMembers?.Any(e => e.groupId == id)).GetValueOrDefault();
        }
    }
}
