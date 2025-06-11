using System.ComponentModel.DataAnnotations;

namespace SwiftStocks.Data.Models
{
	public class Stock
	{
		public int Id { get; set; }
		[Required]
		public string Symbol { get; set; }
		public virtual ICollection<WatchlistStock> WatchlistStocks { get; set; }

	}
}
