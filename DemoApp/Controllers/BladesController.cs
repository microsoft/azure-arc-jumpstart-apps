using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoApp.Data;
using DemoApp.Models;

namespace DemoApp.Controllers
{
    public class BladesController : Controller
    {
        private readonly WindFarmContext _context;

        public BladesController(WindFarmContext context)
        {
            _context = context;
        }

        // GET: Blades
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blades.ToListAsync());
        }

        // GET: Blades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blade = await _context.Blades
                .FirstOrDefaultAsync(m => m.ID == id);
            if (blade == null)
            {
                return NotFound();
            }

            return View(blade);
        }

        // GET: Blades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Length,Position,DateInstalled")] Blade blade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blade);
        }

        // GET: Blades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blade = await _context.Blades.FindAsync(id);
            if (blade == null)
            {
                return NotFound();
            }
            return View(blade);
        }

        // POST: Blades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Length,Position,DateInstalled")] Blade blade)
        {
            if (id != blade.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BladeExists(blade.ID))
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
            return View(blade);
        }

        // GET: Blades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blade = await _context.Blades
                .FirstOrDefaultAsync(m => m.ID == id);
            if (blade == null)
            {
                return NotFound();
            }

            return View(blade);
        }

        // POST: Blades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blade = await _context.Blades.FindAsync(id);
            _context.Blades.Remove(blade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BladeExists(int id)
        {
            return _context.Blades.Any(e => e.ID == id);
        }
    }
}
