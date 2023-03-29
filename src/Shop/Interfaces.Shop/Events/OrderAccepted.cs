namespace Interfaces.Shop.Events;

public class OrderAccepted
{
    public OrderAccepted()
    { }

    public OrderAccepted(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
}