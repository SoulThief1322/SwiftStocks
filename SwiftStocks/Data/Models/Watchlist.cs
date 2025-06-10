using Microsoft.AspNetCore.Identity;
using SwiftStocks.Data.Constants;
using SwiftStocks.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Watchlist
{
    public int Id { get; set; }
    public string UserId { get; set; }
    [Required]
    [MaxLength(WatchlistConstants.NameMaxLength)]
    [MinLength(WatchlistConstants.NameMinLength)]
    public string Name { get; set; }
    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; }
	public virtual ICollection<WatchlistStock> WatchlistStocks { get; set; } = new List<WatchlistStock>();
}