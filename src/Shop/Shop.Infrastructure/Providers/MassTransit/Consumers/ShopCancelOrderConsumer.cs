using Interfaces.Shop.Commands;
using Interfaces.Stock.Commands;
using MassTransit;
using MediatR;
using Shop.MediatR.Contracts.Requests;

namespace Shop.Infrastructure.Providers.MassTransit.Consumers;

public class ShopCancelOrderConsumer : IConsumer<ShopCancelOrderCommand>
{
    private readonly IMediator _mediator;
    private readonly IBusControl _busControl;

    public ShopCancelOrderConsumer(IMediator mediator, 
        IBusControl busControl)
    {
        _mediator = mediator;
        _busControl = busControl;
    }

    public async Task Consume(ConsumeContext<ShopCancelOrderCommand> context)
    {
        var mediatrRequest = new GetOrdersRequest(orderIds: new[] { context.Message.OrderId });
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        var orderInfo = mediatrResponse.Orders.First();

        var stockCancelReservationProductCommand = new StockCancelReservationProductCommand(orderInfo.OrderId,
            orderInfo.ProductId,
            orderInfo.Quantity);

        var sendEndpoint = await _busControl.GetPublishSendEndpoint<StockCancelReservationProductCommand>();
        await sendEndpoint.Send(stockCancelReservationProductCommand);
    }
}