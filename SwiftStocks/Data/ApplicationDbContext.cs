using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SwiftStocks.Data.Models;

namespace SwiftStocks.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Watchlist> Watchlists { get; set; }
        public DbSet<WatchlistStock> WatchlistStocks { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<News> NewsItems { get; set; }
        public DbSet<BoughtStock> BoughtStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WatchlistStock>()
    .HasKey(ws => new { ws.WatchlistId, ws.StockId });

            builder.Entity<WatchlistStock>()
    .HasOne(ws => ws.Watchlist)
    .WithMany(w => w.WatchlistStocks)
    .HasForeignKey(ws => ws.WatchlistId);


            builder.Entity<WatchlistStock>()
                .HasOne(ws => ws.Stock)
                .WithMany(s => s.WatchlistStocks)
                .HasForeignKey(ws => ws.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BoughtStock>()
                .HasIndex(bs => new { bs.UserId, bs.StockSymbol })
                .HasDatabaseName("IX_User_StockSymbol");

            builder.Entity<News>()
                .HasOne(n => n.Author)
                .WithMany()
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BoughtStock>()
.HasOne(bs => bs.User)
.WithMany()
.HasForeignKey(bs => bs.UserId)
.OnDelete(DeleteBehavior.Cascade);

        }
    }
}
