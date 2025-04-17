using Dealship.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Dealship.Controllers
{
    public class MainController : Controller
    {
        private readonly AppDbContext _context;

        public MainController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cars = _context.Cars
                .Where(c => !string.IsNullOrEmpty(c.Image))
                .ToList();

            var random = new Random();
            var selectedCars = cars.OrderBy(c => random.Next()).Take(2).ToList();

            return View(selectedCars); 
        }
    }
}
