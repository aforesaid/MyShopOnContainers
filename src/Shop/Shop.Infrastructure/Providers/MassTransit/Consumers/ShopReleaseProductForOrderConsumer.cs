using Interfaces.Shop.Commands;
using Interfaces.Stock.Commands;
using MassTransit;
using MediatR;
using Shop.MediatR.Contracts.Requests;

namespace Shop.Infrastructure.Providers.MassTransit.Consumers;

public class ShopReleaseProductForOrderConsumer : IConsumer<ShopReleaseProductForOrderCommand>
{
    private readonly IMediator _mediator;
    private readonly IBusControl _busControl;

    public ShopReleaseProductForOrderConsumer(IMediator mediator,
        IBusControl busControl)
    {
        _mediator = mediator;
        _busControl = busControl;
    }

    public async Task Consume(ConsumeContext<ShopReleaseProductForOrderCommand> context)
    {
        var mediatrRequest = new GetOrdersRequest(orderIds: new[] { context.Message.OrderId });
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        var orderInfo = mediatrResponse.Orders.First();

        var stockReleaseProductCommand = new StockReleaseProductCommand(orderInfo.OrderId,
            orderInfo.ProductId,
            orderInfo.Quantity);

        var sendEndpoint = await _busControl.GetPublishSendEndpoint<StockReleaseProductCommand>();
        await sendEndpoint.Send(stockReleaseProductCommand);
    }
}