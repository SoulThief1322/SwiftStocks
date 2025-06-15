using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwiftStocks.Data;
using SwiftStocks.Models;

namespace SwiftStocks.Controllers;

public class StocksController : Controller
{
    private readonly ILogger<StocksController> _logger;
    private readonly ApplicationDbContext _context;

    public StocksController(ILogger<StocksController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [Route("Stocks")]
    public async Task<IActionResult> Index()
    {
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var watchlists = await _context.Watchlists.Where(x => x.UserId == userId).Include(w => w.WatchlistStocks).ThenInclude(ws => ws.Stock).ToListAsync();
        var model = new StocksViewModel
        {
            Watchlists = watchlists
        };
		return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
