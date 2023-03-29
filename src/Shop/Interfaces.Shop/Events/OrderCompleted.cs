namespace Interfaces.Shop.Events;

public class OrderCompleted
{
    public OrderCompleted()
    { }

    public OrderCompleted(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
}