using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dealship.Models;

namespace Dealship.Controllers;

public class AutoController : Controller
{
    private readonly ILogger<AutoController> _logger;

    public AutoController(ILogger<AutoController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}