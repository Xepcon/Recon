using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recon.Data;
using Recon.Models.Model.TimeManager;

namespace Recon.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly DataDbContext _context;

        public AttendancesController(DataDbContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public async Task<IActionResult> Index()
        {
              return _context.Attendances != null ? 
                          View(await _context.Attendances.ToListAsync()) :
                          Problem("Entity set 'DataDbContext.Attendances'  is null.");
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(m => m.AttendanceId == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendanceId,AttendanceName,CreatedAt,userId")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                

                _context.Add(attendance);
                await _context.SaveChangesAsync();
                for (int i = 1; i <= DateTime.DaysInMonth(attendance.CreatedAt.Year, attendance.CreatedAt.Month); i++)
                {
                    AttendanceEntity tmp = new AttendanceEntity();
                    tmp.AttendanceId = attendance.AttendanceId;
                    Debug.WriteLine(attendance.AttendanceId);
                    tmp.Hour = null;

                    tmp.AttendanceDate = new DateTime(attendance.CreatedAt.Year, attendance.CreatedAt.Month, i);
                    tmp.isWeekend = tmp.AttendanceDate.DayOfWeek == DayOfWeek.Saturday || tmp.AttendanceDate.DayOfWeek == DayOfWeek.Sunday;
                    tmp.approved = false;
                    tmp.Minutes = null;
                    tmp.interName = "Kiss Pista";
                    _context.AttendanceEntitys.Add(tmp);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttendanceId,AttendanceName,CreatedAt,userId")] Attendance attendance)
        {
            if (id != attendance.AttendanceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.AttendanceId))
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
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Attendances == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(m => m.AttendanceId == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Attendances == null)
            {
                return Problem("Entity set 'DataDbContext.Attendances'  is null.");
            }
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
          return (_context.Attendances?.Any(e => e.AttendanceId == id)).GetValueOrDefault();
        }
    }
}
