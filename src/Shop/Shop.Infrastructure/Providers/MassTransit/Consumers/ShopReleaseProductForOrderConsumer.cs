using Interfaces.Shop.Commands;
using Interfaces.Stock.Commands;
using MassTransit;
using MediatR;
using Shop.MediatR.Contracts.Requests;

namespace Shop.Infrastructure.Providers.MassTransit.Consumers;

public class ShopReleaseProductForOrderConsumer : IConsumer<ShopReleaseProductForOrderCommand>
{
    private readonly IMediator _mediator;
    private readonly ISendEndpoint _sendEndpoint;

    public ShopReleaseProductForOrderConsumer(IMediator mediator, 
        ISendEndpoint sendEndpoint)
    {
        _mediator = mediator;
        _sendEndpoint = sendEndpoint;
    }

    public async Task Consume(ConsumeContext<ShopReleaseProductForOrderCommand> context)
    {
        var mediatrRequest = new GetOrdersRequest(orderIds: new[] { context.Message.OrderId });
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        var orderInfo = mediatrResponse.Orders.First();

        var stockReleaseProductCommand = new StockReleaseProductCommand(orderInfo.OrderId,
            orderInfo.ProductId,
            orderInfo.Quantity);
        await _sendEndpoint.Send(stockReleaseProductCommand);
    }
}