namespace Interfaces.Stock.Shared;

public class StockProductInfo
{
    public StockProductInfo()
    { }

    public StockProductInfo(Guid productId, 
        string productName,
        int free)
    {
        ProductId = productId;
        ProductName = productName;
        Free = free;
    }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Free { get; set; }
}