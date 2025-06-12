using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SwiftStocks.Models;

namespace SwiftStocks.Controllers;

public class StocksController : Controller
{
    private readonly ILogger<StocksController> _logger;

    public StocksController(ILogger<StocksController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("Stocks")]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
