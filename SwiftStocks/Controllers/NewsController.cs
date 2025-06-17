using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwiftStocks.Data;
using SwiftStocks.Data.Models;
using SwiftStocks.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SwiftStocks.Controllers
{
	[Authorize]
	public class NewsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public NewsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		public async Task<IActionResult> Index()
		{
			var newsEntities = await _context.NewsItems
	.Include(n => n.Author)
	.OrderByDescending(n => n.PublishedAt)
	.ToListAsync();

			var newsItems = newsEntities.Select(n => new NewsListItemViewModel
			{
				Title = n.Title,
				Description = n.Description,
				ImagePath = n.ImagePath,
				PublishedAt = n.PublishedAt,
				SourceName = n.SourceName,
				AuthorName = n.Author?.UserName
			}).ToList();

			return View(newsItems);

		}

		[Authorize(Roles = "Admin")]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateNewsViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			string imagePath = null;

			if (model.ImageFile != null && model.ImageFile.Length > 0)
			{
				var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/news");
				Directory.CreateDirectory(uploadsFolder);

				var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
				var filePath = Path.Combine(uploadsFolder, uniqueFileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					await model.ImageFile.CopyToAsync(fileStream);
				}

				imagePath = "/images/news/" + uniqueFileName;
			}

			var newsItem = new News
			{
				Title = model.Title,
				Description = model.Description,
				PublishedAt = DateTime.UtcNow,
				AuthorId = _userManager.GetUserId(User),
				SourceName = "Custom",
				ImagePath = imagePath
			};

			_context.NewsItems.Add(newsItem);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}
	}
}