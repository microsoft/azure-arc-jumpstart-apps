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
    public class TurbinesController : Controller
    {
        private readonly WindFarmContext _context;

        public TurbinesController(WindFarmContext context)
        {
            _context = context;
        }

        // GET: Turbines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Turbines.ToListAsync());
        }

        // GET: Turbines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turbine = await _context.Turbines
                .FirstOrDefaultAsync(m => m.ID == id);
            if (turbine == null)
            {
                return NotFound();
            }

            return View(turbine);
        }

        // GET: Turbines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Turbines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Manufacturer,Model,DateLastServiced")] Turbine turbine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turbine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(turbine);
        }

        // GET: Turbines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turbine = await _context.Turbines.FindAsync(id);
            if (turbine == null)
            {
                return NotFound();
            }
            return View(turbine);
        }

        // POST: Turbines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Manufacturer,Model,DateLastServiced")] Turbine turbine)
        {
            if (id != turbine.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turbine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurbineExists(turbine.ID))
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
            return View(turbine);
        }

        // GET: Turbines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turbine = await _context.Turbines
                .FirstOrDefaultAsync(m => m.ID == id);
            if (turbine == null)
            {
                return NotFound();
            }

            return View(turbine);
        }

        // POST: Turbines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turbine = await _context.Turbines.FindAsync(id);
            _context.Turbines.Remove(turbine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurbineExists(int id)
        {
            return _context.Turbines.Any(e => e.ID == id);
        }
    }
}
