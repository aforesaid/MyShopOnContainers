using Interfaces.Stock.Events;
using Interfaces.Stock.Messages;
using Interfaces.Stock.Requests;
using MassTransit;
using MediatR;
using Stock.MediatR.Contracts.Commands.SupplyProduct;

namespace Stock.Infrastructure.Providers.MassTransit.Consumers.SupplyProduct;

public class StockSupplyProductConsumer : IConsumer<StockSupplyProductRequest>
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;

    public StockSupplyProductConsumer(IMediator mediator, 
        IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<StockSupplyProductRequest> context)
    {
        var request = context.Message;

        var mediatrRequest = new SupplyProductCommand(request.ProductId,
            request.Quantity);
        var mediatrResponse = await _mediator.Send(mediatrRequest);
        if (mediatrResponse)
        {
            var message = new StockNewSupply(request.ProductId,
                request.Quantity);
            await _publishEndpoint.Publish<StockNewSupply>(message);
        }

        var response = new StockSupplyProductResponse(success: mediatrResponse);
        await context.RespondAsync<StockSupplyProductResponse>(response);
    }
}

public class StockSupplyProductConsumerDefinition : ConsumerDefinition<StockSupplyProductConsumer>
{
    private const int ConcurrencyLimit = 15;
    private static readonly TimeSpan TimeOut = TimeSpan.FromMinutes(5);

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<StockSupplyProductConsumer> consumerConfigurator)
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