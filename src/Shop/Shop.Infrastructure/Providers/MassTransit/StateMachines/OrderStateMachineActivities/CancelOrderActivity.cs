using Interfaces.Shop.Commands;
using Interfaces.Shop.Events;
using MassTransit;
using Shop.Infrastructure.Providers.MassTransit.Consumers;

namespace Shop.Infrastructure.Providers.MassTransit.StateMachines.OrderStateMachineActivities;

public class CancelOrderActivity
    : IStateMachineActivity<OrderState, OrderCancellationStarted>

{
    public void Probe(ProbeContext context)
    {
        context.CreateScope("cancel-order");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<OrderState, OrderCancellationStarted> context, IBehavior<OrderState, OrderCancellationStarted> next)
    {
        var endpointNameConfiguration = context.GetServiceOrCreateInstance<IEndpointAddressProvider>();
        var consumerEndpoint = endpointNameConfiguration
            .GetConsumerEndpoint<ShopCancelOrderConsumer, ShopCancelOrderCommand>();
                    
        var sendEndpoint = await context.GetSendEndpoint(consumerEndpoint);

        var sendRequest = new ShopCancelOrderCommand(context.Message.OrderId);
        await sendEndpoint.Send(sendRequest);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderCancellationStarted, TException> context, IBehavior<OrderState, OrderCancellationStarted> next) where TException : Exception
    {
        return next.Faulted(context);
    }
}