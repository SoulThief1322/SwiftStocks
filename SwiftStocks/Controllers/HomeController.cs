using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SwiftStocks.Models;

namespace SwiftStocks.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public async Task<IActionResult> Privacy()
    {
        return View();
    }
    public async Task<IActionResult> Contacts()
    {
        return View();
    }
    public async Task<IActionResult> About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
