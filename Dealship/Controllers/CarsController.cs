using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dealship.Models;

namespace Dealship.Controllers
{
    public class CarsController : Controller
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Cars.Include(c => c.Color).Include(c => c.Engine).Include(c => c.FuelType).Include(c => c.Transmission);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Color)
                .Include(c => c.Engine)
                .Include(c => c.FuelType)
                .Include(c => c.Transmission)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Id");
            ViewData["EngineId"] = new SelectList(_context.Engines, "Id", "Id");
            ViewData["FuelTypeId"] = new SelectList(_context.FuelTypes, "Id", "Id");
            ViewData["TransmissionId"] = new SelectList(_context.Transmissions, "Id", "Id");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Brand,Model,Year,Price,Mileage,EngineId,TransmissionId,FuelTypeId,ColorId,Image")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Id", car.ColorId);
            ViewData["EngineId"] = new SelectList(_context.Engines, "Id", "Id", car.EngineId);
            ViewData["FuelTypeId"] = new SelectList(_context.FuelTypes, "Id", "Id", car.FuelTypeId);
            ViewData["TransmissionId"] = new SelectList(_context.Transmissions, "Id", "Id", car.TransmissionId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Id", car.ColorId);
            ViewData["EngineId"] = new SelectList(_context.Engines, "Id", "Id", car.EngineId);
            ViewData["FuelTypeId"] = new SelectList(_context.FuelTypes, "Id", "Id", car.FuelTypeId);
            ViewData["TransmissionId"] = new SelectList(_context.Transmissions, "Id", "Id", car.TransmissionId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Model,Year,Price,Mileage,EngineId,TransmissionId,FuelTypeId,ColorId,Image")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
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
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Id", car.ColorId);
            ViewData["EngineId"] = new SelectList(_context.Engines, "Id", "Id", car.EngineId);
            ViewData["FuelTypeId"] = new SelectList(_context.FuelTypes, "Id", "Id", car.FuelTypeId);
            ViewData["TransmissionId"] = new SelectList(_context.Transmissions, "Id", "Id", car.TransmissionId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Color)
                .Include(c => c.Engine)
                .Include(c => c.FuelType)
                .Include(c => c.Transmission)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
