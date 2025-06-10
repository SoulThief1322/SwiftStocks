namespace SwiftStocks.Data.Constants
{
    public static class BoughtStockConstants
    {
        public const int StockSymbolMaxLength = 10;
        public const int StockSymbolMinLength = 1;
        
        public const int StockNameMaxLength = 100;
        public const int StockNameMinLength = 1;

        public const decimal DefaultPurchasePrice = 0.0m;
        public const int DefaultQuantity = 1;

        public static readonly DateTime DefaultPurchaseDate = DateTime.UtcNow;
    }
}