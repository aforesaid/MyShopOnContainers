namespace Interfaces.Shop.Events;

public class OrderFaulted
{
    public OrderFaulted()
    { }

    public OrderFaulted(Guid orderId, string reason = null)
    {
        OrderId = orderId;
        Reason = reason;
    }
    public Guid OrderId { get; set; }
    public string Reason { get; set; }
}