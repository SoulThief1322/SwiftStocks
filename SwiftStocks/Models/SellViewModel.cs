namespace SwiftStocks.Models
{
    public class SellViewModel
    {
        public string StockSymbol { get; set; }
        public string StockName { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}
