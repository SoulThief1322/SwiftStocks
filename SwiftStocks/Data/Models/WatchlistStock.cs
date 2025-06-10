using System.ComponentModel.DataAnnotations.Schema;

namespace SwiftStocks.Data.Models
{
	public class WatchlistStock
	{
		public int WatchlistId { get; set; }
		[ForeignKey(nameof(WatchlistId))]
		public virtual Watchlist Watchlist { get; set; }
		public int StockId { get; set; }
		[ForeignKey(nameof(StockId))]
		public virtual Stock Stock { get; set; }
	}
}
