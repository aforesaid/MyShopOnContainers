using Interfaces.Shop.Commands;
using Interfaces.Shop.Events;
using MassTransit;
using Shop.Infrastructure.Providers.MassTransit.Consumers;

namespace Shop.Infrastructure.Providers.MassTransit.StateMachines.OrderStateMachineActivities;

public class AcceptOrderActivity
: IStateMachineActivity<OrderState, OrderCreated>
{
    public void Probe(ProbeContext context)
    {
        context.CreateScope("accept-order");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<OrderState, OrderCreated> context, IBehavior<OrderState, OrderCreated> next)
    {
        var endpointNameConfiguration = context.GetServiceOrCreateInstance<IEndpointAddressProvider>();
        var consumerEndpoint = endpointNameConfiguration
            .GetConsumerEndpoint<ShopReserveProductForOrderConsumer, ShopReserveProductForOrderCommand>();
                    
        var sendRequest = new ShopReserveProductForOrderCommand(context.Message.OrderId);

        var consumeContext = context.GetPayload<ConsumeContext>();
        var sendEndpoint = await consumeContext.GetSendEndpoint(consumerEndpoint);
                    
        await sendEndpoint.Send(sendRequest);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderCreated, TException> context, IBehavior<OrderState, OrderCreated> next) where TException : Exception
    {
        return next.Faulted(context);
    }
}