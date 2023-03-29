namespace Shop.Infrastructure.Providers.MassTransit.CouriersActivities;

public class OrderStockStatusActivityArguments
{
    public OrderStockStatusActivityArguments()
    { }

    public OrderStockStatusActivityArguments(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
}