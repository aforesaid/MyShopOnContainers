namespace Interfaces.Stock.Commands;

public class StockCancelReleaseProductCommand
{
    public StockCancelReleaseProductCommand()
    { }
    
    public StockCancelReleaseProductCommand(Guid orderId,
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