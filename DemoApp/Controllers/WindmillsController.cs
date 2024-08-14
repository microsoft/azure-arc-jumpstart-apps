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
    public class WindmillsController : Controller
    {
        private readonly WindFarmContext _context;

        public WindmillsController(WindFarmContext context)
        {
            _context = context;
        }

        // GET: Windmills
        public async Task<IActionResult> Index()
        {
            return View(await _context.Windmills.ToListAsync());
        }

        // GET: Windmills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var windmill = await _context.Windmills
                .FirstOrDefaultAsync(m => m.ID == id);
            if (windmill == null)
            {
                return NotFound();
            }

            return View(windmill);
        }

        // GET: Windmills/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Windmills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Model,Manufacturer,DateOfLastMaintenance")] Windmill windmill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(windmill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(windmill);
        }

        // GET: Windmills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var windmill = await _context.Windmills.FindAsync(id);
            if (windmill == null)
            {
                return NotFound();
            }
            return View(windmill);
        }

        // POST: Windmills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Model,Manufacturer,DateOfLastMaintenance")] Windmill windmill)
        {
            if (id != windmill.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(windmill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WindmillExists(windmill.ID))
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
            return View(windmill);
        }

        // GET: Windmills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var windmill = await _context.Windmills
                .FirstOrDefaultAsync(m => m.ID == id);
            if (windmill == null)
            {
                return NotFound();
            }

            return View(windmill);
        }

        // POST: Windmills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var windmill = await _context.Windmills.FindAsync(id);
            _context.Windmills.Remove(windmill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WindmillExists(int id)
        {
            return _context.Windmills.Any(e => e.ID == id);
        }
    }
}
