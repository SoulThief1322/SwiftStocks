using SwiftStocks.Data.Models;

namespace SwiftStocks.Models
{
    public class WatchlistViewModel
    {
        public string Name { get; set; }
        public IEnumerable<WatchlistStock> WatchlistStocks { get; set; }

    }
}