using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;

namespace Shop.Infrastructure.Providers.MassTransit.BatchConsumers;

public class RoutingSlipBatchEventConsumer
    : IConsumer<Batch<RoutingSlipCompleted>>

{
    private readonly ILogger<RoutingSlipBatchEventConsumer> _logger;

    public RoutingSlipBatchEventConsumer(ILogger<RoutingSlipBatchEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Batch<RoutingSlipCompleted>> context)
    {
        _logger.LogInformation("Routing Slips Completed: {TrackingNumbers}",
            string.Join(", ", context.Message.Select(x => x.Message.TrackingNumber)));
        return Task.CompletedTask;
    }
}

public class RoutingSlipBatchEventConsumerDefinition :
    ConsumerDefinition<RoutingSlipBatchEventConsumer>
{
    public RoutingSlipBatchEventConsumerDefinition()
    {
        Endpoint(e => e.PrefetchCount = 20);
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<RoutingSlipBatchEventConsumer> consumerConfigurator)
    {
        consumerConfigurator.Options<BatchOptions>(o => o.SetMessageLimit(10).SetTimeLimit(100));
    }
}