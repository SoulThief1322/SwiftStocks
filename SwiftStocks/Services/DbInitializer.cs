using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SwiftStocks.Data;
using SwiftStocks.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        string[] roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var usersData = new[]
        {
            new { Email = "admin@swiftstocks.com", Password = "Admin123!", Role = "Admin", UserName = "admin" },
            new { Email = "john.doe@example.com", Password = "User123!", Role = "User", UserName = "johndoe" },
            new { Email = "jane.smith@example.com", Password = "User123!", Role = "User", UserName = "janesmith" },
            new { Email = "mark.brown@example.com", Password = "User123!", Role = "User", UserName = "markbrown" }
        };

        var users = new IdentityUser[usersData.Length];
        for (int i = 0; i < usersData.Length; i++)
        {
            var ud = usersData[i];
            var user = await userManager.FindByEmailAsync(ud.Email);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = ud.UserName,
                    Email = ud.Email,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, ud.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, ud.Role);
                }
            }
            users[i] = user;
        }

        var adminUser = users.First(u => u.Email == "admin@swiftstocks.com");
        var johnUser = users.First(u => u.Email == "john.doe@example.com");
        var janeUser = users.First(u => u.Email == "jane.smith@example.com");
        var markUser = users.First(u => u.Email == "mark.brown@example.com");

        if (!context.Stocks.Any())
        {
            var stocks = new[]
            {
                new Stock { Symbol = "AAPL" },
                new Stock { Symbol = "MSFT" },
                new Stock { Symbol = "GOOGL" },
                new Stock { Symbol = "TSLA" },
                new Stock { Symbol = "AMZN" },
                new Stock { Symbol = "NFLX" },
                new Stock { Symbol = "NVDA" },
                new Stock { Symbol = "META" }
            };
            context.Stocks.AddRange(stocks);
            await context.SaveChangesAsync();
        }

        var allStocks = context.Stocks.ToList();

        if (!context.Watchlists.Any())
        {
            var watchlists = new[]
            {
                new Watchlist
                {
                    Name = "John's Tech Watchlist",
                    UserId = johnUser.Id
                },
                new Watchlist
                {
                    Name = "Jane's Growth Stocks",
                    UserId = janeUser.Id
                },
                new Watchlist
                {
                    Name = "Mark's Favorite Picks",
                    UserId = markUser.Id
                },
                new Watchlist
                {
                    Name = "Admin's Portfolio",
                    UserId = adminUser.Id
                }
            };
            context.Watchlists.AddRange(watchlists);
            await context.SaveChangesAsync();

            var johnWatchlist = watchlists[0];
            var janeWatchlist = watchlists[1];
            var markWatchlist = watchlists[2];
            var adminWatchlist = watchlists[3];

            var watchlistStocks = new[]
            {
                new WatchlistStock { WatchlistId = johnWatchlist.Id, StockId = allStocks.First(s => s.Symbol == "AAPL").Id },
                new WatchlistStock { WatchlistId = johnWatchlist.Id, StockId = allStocks.First(s => s.Symbol == "MSFT").Id },
                new WatchlistStock { WatchlistId = johnWatchlist.Id, StockId = allStocks.First(s => s.Symbol == "NVDA").Id },

                new WatchlistStock { WatchlistId = janeWatchlist.Id, StockId = allStocks.First(s => s.Symbol == "TSLA").Id },
                new WatchlistStock { WatchlistId = janeWatchlist.Id, StockId = allStocks.First(s => s.Symbol == "AMZN").Id },

                new WatchlistStock { WatchlistId = markWatchlist.Id, StockId = allStocks.First(s => s.Symbol == "META").Id },
                new WatchlistStock { WatchlistId = markWatchlist.Id, StockId = allStocks.First(s => s.Symbol == "NFLX").Id },

                new WatchlistStock { WatchlistId = adminWatchlist.Id, StockId = allStocks.First(s => s.Symbol == "GOOGL").Id },
                new WatchlistStock { WatchlistId = adminWatchlist.Id, StockId = allStocks.First(s => s.Symbol == "AMZN").Id },
                new WatchlistStock { WatchlistId = adminWatchlist.Id, StockId = allStocks.First(s => s.Symbol == "AAPL").Id }
            };
            context.WatchlistStocks.AddRange(watchlistStocks);
            await context.SaveChangesAsync();
        }

        if (!context.BoughtStocks.Any())
        {
            var boughtStocks = new[]
            {
                new BoughtStock
                {
                    StockSymbol = "AAPL",
                    StockName = "Apple Inc.",
                    Quantity = 50,
                    PurchasePrice = 135.50m,
                    PurchaseDate = DateTime.UtcNow.AddMonths(-6),
                    UserId = johnUser.Id
                },
                new BoughtStock
                {
                    StockSymbol = "MSFT",
                    StockName = "Microsoft Corporation",
                    Quantity = 30,
                    PurchasePrice = 250.00m,
                    PurchaseDate = DateTime.UtcNow.AddMonths(-4),
                    UserId = johnUser.Id
                },
                new BoughtStock
                {
                    StockSymbol = "TSLA",
                    StockName = "Tesla Inc.",
                    Quantity = 10,
                    PurchasePrice = 600.00m,
                    PurchaseDate = DateTime.UtcNow.AddMonths(-3),
                    UserId = janeUser.Id
                },
                new BoughtStock
                {
                    StockSymbol = "AMZN",
                    StockName = "Amazon.com Inc.",
                    Quantity = 5,
                    PurchasePrice = 3300.00m,
                    PurchaseDate = DateTime.UtcNow.AddMonths(-5),
                    UserId = markUser.Id
                },
                new BoughtStock
                {
                    StockSymbol = "META",
                    StockName = "Meta Platforms Inc.",
                    Quantity = 20,
                    PurchasePrice = 275.00m,
                    PurchaseDate = DateTime.UtcNow.AddMonths(-2),
                    UserId = adminUser.Id
                }
            };
            context.BoughtStocks.AddRange(boughtStocks);
            await context.SaveChangesAsync();
        }

        if (!context.NewsItems.Any())
        {
            var news = new[]
            {
                new News
                {
                    Title = "Apple Q2 Earnings Beat Expectations",
                    Description = "Apple reported better than expected quarterly earnings, driven by strong iPhone sales and growing services revenue.",
                    PublishedAt = DateTime.UtcNow.AddDays(-7),
                    SourceName = "Reuters",
                    AuthorId = adminUser.Id,
                    IsDeleted = false,
                    ImagePath = "/images/news/apple-q2.jpg"
                },
                new News
                {
                    Title = "Tesla's New Model Unveiled",
                    Description = "Tesla unveiled its new electric vehicle model, promising higher range and improved battery technology.",
                    PublishedAt = DateTime.UtcNow.AddDays(-10),
                    SourceName = "Bloomberg",
                    AuthorId = janeUser.Id,
                    IsDeleted = false,
                    ImagePath = "/images/news/tesla-model.jpg"
                },
                new News
                {
                    Title = "Amazon Expands Into Healthcare",
                    Description = "Amazon announced expansion plans into the healthcare market, aiming to disrupt traditional services.",
                    PublishedAt = DateTime.UtcNow.AddDays(-15),
                    SourceName = "CNBC",
                    AuthorId = markUser.Id,
                    IsDeleted = false,
                    ImagePath = "/images/news/amazon-healthcare.jpg"
                },
                new News
                {
                    Title = "Meta Reports User Growth",
                    Description = "Meta Platforms reported significant growth in monthly active users, driven by new features on Instagram and Facebook.",
                    PublishedAt = DateTime.UtcNow.AddDays(-3),
                    SourceName = "TechCrunch",
                    AuthorId = johnUser.Id,
                    IsDeleted = false,
                    ImagePath = "/images/news/meta-growth.jpg"
                }
            };
            context.NewsItems.AddRange(news);
            await context.SaveChangesAsync();
        }

        if (!context.Ratings.Any())
        {
			var newsList = context.NewsItems.ToList();

            var ratings = new[]
            {
                new Rating
                {
                    NewsId = newsList.First(n => n.Title.Contains("Apple")).Id,
                    UserId = johnUser.Id,
                    Value = 5,
                    CreatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new Rating
                {
                    NewsId = newsList.First(n => n.Title.Contains("Tesla")).Id,
                    UserId = janeUser.Id,
                    Value = 4,
                    CreatedAt = DateTime.UtcNow.AddDays(-9)
                },
                new Rating
                {
                    NewsId = newsList.First(n => n.Title.Contains("Amazon")).Id,
                    UserId = markUser.Id,
                    Value = 3,
                    CreatedAt = DateTime.UtcNow.AddDays(-14)
                },
                new Rating
                {
                    NewsId = newsList.First(n => n.Title.Contains("Meta")).Id,
                    UserId = adminUser.Id,
                    Value = 5,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Rating
                {
                    NewsId = newsList.First(n => n.Title.Contains("Apple")).Id,
                    UserId = adminUser.Id,
                    Value = 4,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                }
            };
            context.Ratings.AddRange(ratings);
            await context.SaveChangesAsync();
        }
    }
}
