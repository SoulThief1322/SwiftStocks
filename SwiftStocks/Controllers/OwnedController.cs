using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwiftStocks.Data;
using SwiftStocks.Models;
using System.Security.Claims;

namespace SwiftStocks.Controllers
{
    public class OwnedController : Controller
    {
        private readonly ApplicationDbContext _context;
		public OwnedController(ApplicationDbContext context)
		{
            _context = context;
		}
		public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var owned = await _context.BoughtStocks.Where(x => x.UserId == userId).ToListAsync();
            var ownedViewModel = new OwnedViewModel
            {
                OwnedStocks = owned
            };
            return View(ownedViewModel);
        }
    }
}
