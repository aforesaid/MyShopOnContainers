using Interfaces.Shop.Events;
using Interfaces.Shop.Requests;
using MassTransit;
using MediatR;
using Shop.MediatR.Contracts.Requests;

namespace Shop.Infrastructure.Providers.MassTransit.Consumers;

public class ShopCreateOrderConsumer : IConsumer<ShopCreateOrderRequest>
{
    private readonly IMediator _mediator;

    public ShopCreateOrderConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ShopCreateOrderRequest> context)
    {
        var request = context.Message;

        try
        {
            var mediatrRequest = new CreateOrderRequest(request.UserId,
                request.ProductId,
                request.Quantity);
        
            var mediatrResponse = await _mediator.Send(mediatrRequest);
        
            var orderId = mediatrResponse.OrderId;
            var response = new ShopCreateOrderResponse(orderId);
            
            await context.Publish(new OrderCreated(orderId));
            await context.RespondAsync(response);

        }
        catch (Exception e)
        {
            var response = new ShopCreateOrderResponse(success: false,
                errorReason: e.InnerException?.Message ?? e.Message);
            await context.RespondAsync(response);
        }
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