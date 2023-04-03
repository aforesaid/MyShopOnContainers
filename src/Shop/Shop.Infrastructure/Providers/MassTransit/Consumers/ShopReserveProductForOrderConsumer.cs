using Interfaces.Shop.Commands;
using Interfaces.Shop.Events;
using MassTransit;
using MassTransit.Courier.Contracts;
using Shop.Infrastructure.Providers.MassTransit.CouriersActivities;

namespace Shop.Infrastructure.Providers.MassTransit.Consumers;

public class ShopReserveProductForOrderConsumer : IConsumer<ShopReserveProductForOrderCommand>
{
    private readonly IEndpointAddressProvider _provider;

    public ShopReserveProductForOrderConsumer(IEndpointAddressProvider provider)
    {
        _provider = provider;
    }

    public async Task Consume(ConsumeContext<ShopReserveProductForOrderCommand> context)
    {
        var request = context.Message;
        
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        
        builder.AddActivity(nameof(OrderStockStatusActivity), _provider.GetExecuteEndpoint<OrderStockStatusActivity, OrderStockStatusActivityArguments>(),
            new OrderStockStatusActivityArguments(orderId: request.OrderId));
        
        builder.AddActivity(nameof(OrderReserveProductActivity), _provider.GetExecuteEndpoint<OrderReserveProductActivity, OrderReserveProductActivityArguments>(),
            new OrderReserveProductActivityArguments(orderId: request.OrderId));
        
        await builder.AddSubscription(context.SourceAddress,
            RoutingSlipEvents.Faulted | RoutingSlipEvents.Supplemental,
            RoutingSlipEventContents.None,
            x => x.Send(new OrderFaulted(request.OrderId)));

        var routingSlip = builder.Build();

        await context.Execute(routingSlip);
    }
}