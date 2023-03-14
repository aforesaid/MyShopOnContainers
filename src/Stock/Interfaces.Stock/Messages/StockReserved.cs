namespace Interfaces.Stock.Messages;

public class StockReserved
{
    public StockReserved()
    { }

    public StockReserved(Guid orderId,
        Guid productId, 
        int productCount)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductCount = productCount;
    }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int ProductCount { get; set; }
}