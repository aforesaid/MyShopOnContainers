namespace Interfaces.Stock.Messages;

public class StockOutOfStock
{
    public StockOutOfStock()
    { }

    public StockOutOfStock(Guid orderId,
        Guid productId,
        int quantity)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
    }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}