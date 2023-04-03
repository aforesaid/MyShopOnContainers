namespace Interfaces.Shop.Events;

public class OrderCreated
{
    public OrderCreated()
    { }

    public OrderCreated(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
}
    
