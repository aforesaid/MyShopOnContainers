namespace Interfaces.Stock.Messages;

public class StockNewSupply
{
    public StockNewSupply()
    { }

    public StockNewSupply(Guid productId, 
        int productCount)
    {
        ProductId = productId;
        ProductCount = productCount;
    }
    public Guid ProductId { get; set; }
    public int ProductCount { get; set; }
}