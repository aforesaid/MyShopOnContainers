using Interfaces.Shop.Enums;
using Interfaces.Shop.Requests;
using Interfaces.Shop.Shared;
using MassTransit;
using MediatR;
using Shop.MediatR.Contracts.Requests;

namespace Shop.Infrastructure.Providers.MassTransit.Consumers;

public class ShopGetOrdersConsumer : IConsumer<ShopGetOrdersRequest>
{
    private readonly IMediator _mediator;

    public ShopGetOrdersConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<ShopGetOrdersRequest> context)
    {
        var request = context.Message;

        var mediatrRequest = new GetOrdersRequest(request.UserIds);
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        var result = new ShopGetOrdersResponse(mediatrResponse.Orders
            .Select(x => new ShopOrderInfo(x.UserId,
                x.OrderId,
                x.ProductId,
                x.Quantity,
                (ShopOrderStatesEnum) x.OrderState,
                x.Created,
                x.Updated)));
        await context.RespondAsync(result);
    }
}

public class ShopGetOrdersConsumerDefinition : ConsumerDefinition<ShopGetOrdersConsumer>
{
    private const int ConcurrencyLimit = 15;
    private static readonly TimeSpan TimeOut = TimeSpan.FromMinutes(5);

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ShopGetOrdersConsumer> consumerConfigurator)
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