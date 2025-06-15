using System.Collections.Generic;
using SwiftStocks.Data.Models;
namespace SwiftStocks.Models;



public class OwnedViewModel
{
    public IEnumerable<BoughtStock> OwnedStocks { get; set; }
    public decimal TotalValue => OwnedStocks.Sum(stock => stock.Quantity * stock.PurchasePrice);
    
}