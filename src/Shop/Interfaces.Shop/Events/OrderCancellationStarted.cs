namespace Interfaces.Shop.Events;

public class OrderCancellationStarted
{
    public OrderCancellationStarted()
    { }

    public OrderCancellationStarted(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
}