using Interfaces.Stock.Commands;
using Interfaces.Stock.Events;
using MassTransit;
using MediatR;
using Stock.MediatR.Contracts.Requests;

namespace Stock.Infrastructure.Providers.MassTransit.Consumers;

public class StockCancelReservationProductConsumer : IConsumer<StockCancelReservationProductCommand>
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;

    public StockCancelReservationProductConsumer(IMediator mediator,
        IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<StockCancelReservationProductCommand> context)
    {
        var request = context.Message;
        
        var mediatrRequest = new CancelReserveProductRequest(request.ProductId,
            request.Quantity);
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        if (mediatrResponse.Success)
        {
            var reserveCancelledStockEvent = new StockReservationCancelled(request.OrderId,
                request.ProductId,
                request.Quantity);
            await _publishEndpoint.Publish(reserveCancelledStockEvent);
        }
    }
}