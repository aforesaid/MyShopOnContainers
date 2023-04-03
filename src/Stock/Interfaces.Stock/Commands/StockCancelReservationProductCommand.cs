namespace Interfaces.Stock.Commands;

public class StockCancelReservationProductCommand
{
    public StockCancelReservationProductCommand()
    { }
    
    public StockCancelReservationProductCommand(Guid orderId,
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