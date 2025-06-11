using Microsoft.AspNetCore.Identity;
using SwiftStocks.Data.Constants;
using SwiftStocks.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SwiftStocks.Data.Models
{
    public class BoughtStock
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(BoughtStockConstants.StockSymbolMaxLength)]
        [MinLength(BoughtStockConstants.StockSymbolMinLength)]
        public string StockSymbol { get; set; }
        [Required]
        [MaxLength(BoughtStockConstants.StockNameMaxLength)]
        [MinLength(BoughtStockConstants.StockNameMinLength)]
        public string StockName { get; set; }
        [Required]
        public int Quantity { get; set; } = BoughtStockConstants.DefaultQuantity;
        [Required]
        public decimal PurchasePrice { get; set; } = BoughtStockConstants.DefaultPurchasePrice;
        [Required]
        public DateTime PurchaseDate { get; set; } = BoughtStockConstants.DefaultPurchaseDate;
        
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }

    }
}