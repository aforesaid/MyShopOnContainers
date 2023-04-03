namespace Shop.Infrastructure.Providers.MassTransit.CouriersActivities;

public class OrderAcceptActivityArguments
{ 
    public OrderAcceptActivityArguments()
    { }

    public OrderAcceptActivityArguments(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
}