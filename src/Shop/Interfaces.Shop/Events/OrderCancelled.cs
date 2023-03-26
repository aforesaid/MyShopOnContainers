namespace Interfaces.Shop.Events;

public class OrderCancelled
{
    public OrderCancelled()
    { }

    public OrderCancelled(Guid userId, Guid orderId)
    {
        UserId = userId;
        OrderId = orderId;
    }
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}