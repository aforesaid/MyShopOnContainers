namespace Shop.Infrastructure.Providers.MassTransit.CouriersActivities;

public class OrderReserveProductActivityArguments
{ 
    public OrderReserveProductActivityArguments()
    { }

    public OrderReserveProductActivityArguments(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
}