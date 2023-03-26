using Interfaces.Shop.Requests;
using MassTransit;

namespace Shop.Infrastructure.Providers.MassTransit.Consumers;

public class ShopCreateOrderConsumer : IConsumer<ShopCreateOrderRequest>
{
    public Task Consume(ConsumeContext<ShopCreateOrderRequest> context)
    {
        throw new NotImplementedException();
    }
}

public class ShopCreateOrderConsumerDefinition : ConsumerDefinition<ShopCreateOrderConsumer>
{
    private const int ConcurrencyLimit = 15;
    private static readonly TimeSpan TimeOut = TimeSpan.FromMinutes(5);

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ShopCreateOrderConsumer> consumerConfigurator)
    {
        endpointConfigurator.DiscardFaultedMessages();
        
        endpointConfigurator.PrefetchCount = ConcurrencyLimit;
        endpointConfigurator.UseConcurrencyLimit(ConcurrencyLimit);
        
        consumerConfigurator.UseConcurrencyLimit(ConcurrencyLimit);
        consumerConfigurator.UseTimeout(cfg =>
        {
            cfg.Timeout = TimeOut;
        });
    }
}