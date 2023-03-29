namespace Interfaces.Stock.Commands;

public class StockReleaseProductCommand
{
    public StockReleaseProductCommand()
    { }
    
    public StockReleaseProductCommand(Guid orderId, 
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