using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;

namespace Shop.Infrastructure.Providers.MassTransit.BatchConsumers;

public class RoutingSlipBatchFaultedEventConsumer
    : IConsumer<Batch<RoutingSlipFaulted>>
{
    private readonly ILogger<RoutingSlipBatchFaultedEventConsumer> _logger;

    public RoutingSlipBatchFaultedEventConsumer(ILogger<RoutingSlipBatchFaultedEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Batch<RoutingSlipFaulted>> context)
    {
        _logger.LogInformation("Routing Slips Faulted: {TrackingNumbers}",
            string.Join(", ", context.Message.Select(x => x.Message.TrackingNumber)));
        return Task.CompletedTask;
    }
}

public class RoutingSlipBatchFaultedEventConsumerDefinition :
    ConsumerDefinition<RoutingSlipBatchFaultedEventConsumer>
{
    public RoutingSlipBatchFaultedEventConsumerDefinition()
    {
        Endpoint(e => e.PrefetchCount = 20);
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<RoutingSlipBatchFaultedEventConsumer> consumerConfigurator)
    {
        consumerConfigurator.Options<BatchOptions>(o => o.SetMessageLimit(10).SetTimeLimit(100));
    }
}