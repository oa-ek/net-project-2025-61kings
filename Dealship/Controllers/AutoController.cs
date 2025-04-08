using Microsoft.EntityFrameworkCore;
using Dealship.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class AutoController : Controller
{
    private readonly ILogger<AutoController> _logger;
    private readonly AppDbContext _context;

    public AutoController(ILogger<AutoController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var cars = await _context.Cars
            .Include(c => c.Engine)
            .Include(c => c.Transmission)
            .Include(c => c.FuelType)
            .Include(c => c.Color)
            .ToListAsync();

        return View(cars);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
