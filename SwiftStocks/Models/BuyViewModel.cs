using System.ComponentModel.DataAnnotations;
namespace SwiftStocks.Models
{
	public class BuyViewModel
	{
		[Required]
		public string StockSymbol { get; set; }

		[Required]
		public string StockName { get; set; }

		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
		public int Quantity { get; set; }

		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
		public decimal PurchasePrice { get; set; }

		public DateTime PurchaseDate = DateTime.UtcNow;

		public string UserId { get; set; }
	}
}
