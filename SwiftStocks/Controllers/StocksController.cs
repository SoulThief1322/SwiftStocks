using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwiftStocks.Data;
using SwiftStocks.Data.Models;
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
        var watchlists = await _context.Watchlists
            .Where(x => x.UserId == userId)
            .Include(w => w.WatchlistStocks)
            .ThenInclude(ws => ws.Stock)
            .ToListAsync();

        var model = new StocksViewModel
        {
            Watchlists = watchlists
        };
        return View(model);
    }

    [HttpGet]
    [Route("Stocks/Buy")]
    public IActionResult Buy(string symbol, string name, decimal price)
    {
        if (string.IsNullOrEmpty(symbol))
            return BadRequest("Stock symbol is required.");

        var model = new BuyViewModel
        {
            StockSymbol = symbol,
            StockName = name,
            PurchasePrice = price,
            Quantity = 1,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            PurchaseDate = DateTime.UtcNow
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Stocks/Buy")]
    public async Task<IActionResult> Buy(BuyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        model.UserId = userId;
        model.PurchaseDate = DateTime.UtcNow;

        var boughtStock = new BoughtStock
        {
            StockSymbol = model.StockSymbol,
            StockName = model.StockName,
            PurchasePrice = model.PurchasePrice,
            Quantity = model.Quantity,
            PurchaseDate = model.PurchaseDate,
            UserId = model.UserId,

        };
        _context.BoughtStocks.Add(boughtStock);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"User {userId} purchased {model.Quantity} shares of {model.StockSymbol} at {model.PurchasePrice}");

        return RedirectToAction("Index", "Stocks");
    }

    [HttpGet]
    [Route("Stocks/Sell")]
    public async Task<IActionResult> Sell(string symbol)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        if (string.IsNullOrWhiteSpace(symbol)) return BadRequest("Stock symbol required.");

        var holding = await _context.BoughtStocks
            .FirstOrDefaultAsync(b => b.StockSymbol == symbol && b.UserId == userId);

        if (holding == null)
        {
            return NotFound("You don't own this stock.");
        }

        var model = new SellViewModel
        {
            StockSymbol = holding.StockSymbol,
            StockName = holding.StockName,
            Quantity = holding.Quantity,
            PurchasePrice = holding.PurchasePrice
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Stocks/Sell")]
    public async Task<IActionResult> SellConfirmed(SellViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var stock = await _context.BoughtStocks
            .FirstOrDefaultAsync(b => b.StockSymbol == model.StockSymbol && b.UserId == userId);

        if (stock == null)
        {
            return NotFound("Stock not found or already sold.");
        }

        _context.BoughtStocks.Remove(stock);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"User {userId} sold {stock.Quantity} shares of {stock.StockSymbol}");

        return RedirectToAction("Index", "Stocks");
    }
    [HttpGet]
    [Route("Stocks/CheckOwnership")]
    public async Task<IActionResult> CheckOwnership(string symbol)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        if (string.IsNullOrWhiteSpace(symbol))
            return BadRequest("Missing symbol.");

        var holding = await _context.BoughtStocks
            .FirstOrDefaultAsync(b => b.StockSymbol == symbol && b.UserId == userId);

        return holding != null ? Ok() : NotFound("You don't own this stock.");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
