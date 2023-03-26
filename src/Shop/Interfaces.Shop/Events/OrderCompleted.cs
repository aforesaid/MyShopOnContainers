namespace Interfaces.Shop.Events;

public class OrderCompleted
{
    public OrderCompleted()
    { }

    public OrderCompleted(Guid userId, 
        Guid orderId)
    {
        UserId = userId;
        OrderId = orderId;
    }
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}