namespace Interfaces.Shop.Events;

public class OrderCancelled
{
    public OrderCancelled()
    { }

    public OrderCancelled(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
}