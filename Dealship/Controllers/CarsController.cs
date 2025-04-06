using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dealship.Models;
using Microsoft.Extensions.Logging;

namespace Dealship.Controllers
{
    public class CarsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CarsController> _logger;
        private readonly string _imageFolder = "wwwroot/images";

        public CarsController(AppDbContext context, ILogger<CarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var cars = _context.Cars
                .Include(c => c.Color)
                .Include(c => c.Engine)
                .Include(c => c.FuelType)
                .Include(c => c.Transmission);
            return View(await cars.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _context.Cars
                .Include(c => c.Color)
                .Include(c => c.Engine)
                .Include(c => c.FuelType)
                .Include(c => c.Transmission)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (car == null)
                return NotFound();

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            PopulateSelectLists();
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Car car)
        {
            _logger.LogInformation($"ImageFile in form: {car.ImageFile?.FileName ?? "null"}");
            _logger.LogInformation($"Request.Form.Files count: {Request.Form.Files.Count}");

            if (!ModelState.IsValid)
            {
                LogModelStateErrors();
                PopulateSelectLists(car);
                return View(car);
            }

            if (car.ImageFile != null && car.ImageFile.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_imageFolder))
                        Directory.CreateDirectory(_imageFolder);

                    var fileName = Path.GetFileName(car.ImageFile.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                    var fullPath = Path.Combine(_imageFolder, uniqueFileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await car.ImageFile.CopyToAsync(stream);
                    }

                    car.Image = "/images/" + uniqueFileName;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[Image Upload] Error: {ex.Message}");
                    ModelState.AddModelError("ImageFile", "Не вдалося завантажити зображення.");
                    PopulateSelectLists(car);
                    return View(car);
                }
            }

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            PopulateSelectLists(car);
            return View(car);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Model,Year,Price,Mileage,EngineId,TransmissionId,FuelTypeId,ColorId,Image")] Car car)
        {
            if (id != car.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                PopulateSelectLists(car);
                return View(car);
            }

            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(car.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _context.Cars
                .Include(c => c.Color)
                .Include(c => c.Engine)
                .Include(c => c.FuelType)
                .Include(c => c.Transmission)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (car == null)
                return NotFound();

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
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        private void PopulateSelectLists(Car car = null)
        {
            ViewBag.EngineId = new SelectList(_context.Engines, "Id", "Type", car?.EngineId);
            ViewBag.TransmissionId = new SelectList(_context.Transmissions, "Id", "Type", car?.TransmissionId);
            ViewBag.FuelTypeId = new SelectList(_context.FuelTypes, "Id", "Name", car?.FuelTypeId);
            ViewBag.ColorId = new SelectList(_context.Colors, "Id", "Name", car?.ColorId);
        }

        private void LogModelStateErrors()
        {
            foreach (var kvp in ModelState)
            {
                foreach (var error in kvp.Value.Errors)
                {
                    _logger.LogError($"[ModelState Error] Field: {kvp.Key}, Error: {error.ErrorMessage}");
                }
            }
        }
    }
}
