using Interfaces.Shop.Commands;
using Interfaces.Shop.Events;
using MassTransit;
using Shop.Infrastructure.Providers.MassTransit.Consumers;

namespace Shop.Infrastructure.Providers.MassTransit.StateMachines.OrderStateMachineActivities;

public class ReleaseOrderActivity
    : IStateMachineActivity<OrderState, OrderReleaseStockStarted>

{
    public void Probe(ProbeContext context)
    {
        context.CreateScope("release-order");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<OrderState, OrderReleaseStockStarted> context, IBehavior<OrderState, OrderReleaseStockStarted> next)
    {
        var endpointNameConfiguration = context.GetServiceOrCreateInstance<IEndpointAddressProvider>();
        var consumerEndpoint = endpointNameConfiguration
            .GetConsumerEndpoint<ShopReleaseProductForOrderConsumer, ShopReleaseProductForOrderCommand>();
                    
        var sendEndpoint = await context.GetSendEndpoint(consumerEndpoint);

        var sendRequest = new ShopReleaseProductForOrderCommand(context.Message.OrderId);
        await sendEndpoint.Send(sendRequest);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderReleaseStockStarted, TException> context, IBehavior<OrderState, OrderReleaseStockStarted> next) where TException : Exception
    {
        return next.Faulted(context);
    }
}