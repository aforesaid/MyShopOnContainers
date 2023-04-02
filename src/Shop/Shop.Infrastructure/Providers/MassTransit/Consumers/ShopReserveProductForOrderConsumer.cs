using Interfaces.Shop.Commands;
using Interfaces.Shop.Events;
using MassTransit;
using MassTransit.Courier.Contracts;
using Shop.Infrastructure.Providers.MassTransit.CouriersActivities;

namespace Shop.Infrastructure.Providers.MassTransit.Consumers;

public class ShopReserveProductForOrderConsumer : IConsumer<ShopReserveProductForOrderCommand>
{
    readonly IEndpointAddressProvider _provider;

    public ShopReserveProductForOrderConsumer(IEndpointAddressProvider provider)
    {
        _provider = provider;
    }

    public async Task Consume(ConsumeContext<ShopReserveProductForOrderCommand> context)
    {
        var request = context.Message;
        
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        
        builder.AddActivity("OrderStockStatus", _provider.GetExecuteEndpoint<OrderStockStatusActivity, OrderStockStatusActivityArguments>(),
            new OrderStockStatusActivityArguments(orderId: request.OrderId));
        
        builder.AddActivity("OrderAccept", _provider.GetExecuteEndpoint<OrderAcceptActivity, OrderAcceptActivityArguments>(),
            new OrderAcceptActivityArguments(orderId: request.OrderId));
        
        await builder.AddSubscription(context.SourceAddress,
            RoutingSlipEvents.Faulted | RoutingSlipEvents.Supplemental,
            RoutingSlipEventContents.None, 
            x => x.Send(new OrderFaulted(request.OrderId)));

        await builder.AddSubscription(context.SourceAddress,
            RoutingSlipEvents.Completed | RoutingSlipEvents.Supplemental,
            RoutingSlipEventContents.None,
            nameof(OrderAcceptActivity),
            x => x.Send(new OrderAccepted(request.OrderId)));

        var routingSlip = builder.Build();

        await context.Execute(routingSlip);
    }
}