using Interfaces.Stock.Commands;
using Interfaces.Stock.Events;
using MassTransit;
using MediatR;
using Stock.Domain.Exceptions;
using Stock.MediatR.Contracts.Requests;

namespace Stock.Infrastructure.Providers.MassTransit.Consumers;

public class StockReserveProductConsumer : IConsumer<StockReserveProductCommand>
{
    private readonly IPublishEndpoint _publishEndpoint;
    
    private readonly IMediator _mediator;

    public StockReserveProductConsumer(IMediator mediator,
        IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<StockReserveProductCommand> context)
    {
        var request = context.Message;

        try
        {
            var mediatrRequest = new ReserveProductRequest(request.ProductId,
                request.Quantity);
            var mediatrResponse = await _mediator.Send(mediatrRequest);

            if (mediatrResponse.Success)
            {
                var stockReservedEvent = new StockReserved(request.OrderId,
                    request.ProductId,
                    request.Quantity);

                await _publishEndpoint.Publish(stockReservedEvent);
            }
        }
        catch (OutOfStockException)
        {
            var outOfStockEvent = new StockOutOfStock(request.OrderId,
                request.ProductId,
                request.Quantity);
            await _publishEndpoint.Publish(outOfStockEvent);
        }
    }
}