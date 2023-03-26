namespace Interfaces.Shop.Events;

public class OrderAccepted
{
    public OrderAccepted()
    { }

    public OrderAccepted(Guid userId, 
        Guid orderId)
    {
        UserId = userId;
        OrderId = orderId;
    }
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}