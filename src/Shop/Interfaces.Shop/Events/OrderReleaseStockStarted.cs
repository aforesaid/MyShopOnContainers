namespace Interfaces.Shop.Events;

public class OrderReleaseStockStarted
{
    public OrderReleaseStockStarted()
    { }

    public OrderReleaseStockStarted(Guid orderId)
    {
        OrderId = orderId;

    }
    public Guid OrderId { get; set; }
}