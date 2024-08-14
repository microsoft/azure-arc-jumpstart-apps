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
    public class TurbineTelemetrySamplesController : Controller
    {
        private readonly WindFarmContext _context;

        public TurbineTelemetrySamplesController(WindFarmContext context)
        {
            _context = context;
        }

        // GET: TurbineTelemetrySamples
        public async Task<IActionResult> Index()
        {
            return View(await _context.TurbineTelemetrySample.ToListAsync());
        }

        // GET: TurbineTelemetrySamples/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turbineTelemetrySample = await _context.TurbineTelemetrySample
                .FirstOrDefaultAsync(m => m.ID == id);
            if (turbineTelemetrySample == null)
            {
                return NotFound();
            }

            return View(turbineTelemetrySample);
        }

        // GET: TurbineTelemetrySamples/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TurbineTelemetrySamples/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GearboxOilLevel,GearboxOilTemp,GeneratorActivePower,GeneratorSpeed,GeneratorTorque,GridFrequency,GridVoltage,HydraulicOilPressure,NacelleAngle,OverallWindDirection,WindSpeedStdDev,Precipitation,TurbineWindDirection,TurbineSpeedStdDev,WindSpeedAverage,WindTempAverage,PitchAngle,Vibration,TurbineSpeedAverage,AlterBlades")] TurbineTelemetrySample turbineTelemetrySample)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turbineTelemetrySample);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(turbineTelemetrySample);
        }

        // GET: TurbineTelemetrySamples/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turbineTelemetrySample = await _context.TurbineTelemetrySample.FindAsync(id);
            if (turbineTelemetrySample == null)
            {
                return NotFound();
            }
            return View(turbineTelemetrySample);
        }

        // POST: TurbineTelemetrySamples/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,GearboxOilLevel,GearboxOilTemp,GeneratorActivePower,GeneratorSpeed,GeneratorTorque,GridFrequency,GridVoltage,HydraulicOilPressure,NacelleAngle,OverallWindDirection,WindSpeedStdDev,Precipitation,TurbineWindDirection,TurbineSpeedStdDev,WindSpeedAverage,WindTempAverage,PitchAngle,Vibration,TurbineSpeedAverage,AlterBlades")] TurbineTelemetrySample turbineTelemetrySample)
        {
            if (id != turbineTelemetrySample.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turbineTelemetrySample);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurbineTelemetrySampleExists(turbineTelemetrySample.ID))
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
            return View(turbineTelemetrySample);
        }

        // GET: TurbineTelemetrySamples/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turbineTelemetrySample = await _context.TurbineTelemetrySample
                .FirstOrDefaultAsync(m => m.ID == id);
            if (turbineTelemetrySample == null)
            {
                return NotFound();
            }

            return View(turbineTelemetrySample);
        }

        // POST: TurbineTelemetrySamples/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var turbineTelemetrySample = await _context.TurbineTelemetrySample.FindAsync(id);
            _context.TurbineTelemetrySample.Remove(turbineTelemetrySample);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurbineTelemetrySampleExists(long id)
        {
            return _context.TurbineTelemetrySample.Any(e => e.ID == id);
        }
    }
}
