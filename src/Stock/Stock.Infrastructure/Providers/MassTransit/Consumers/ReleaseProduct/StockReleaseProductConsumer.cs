using Interfaces.Stock.Commands;
using Interfaces.Stock.Events;
using MassTransit;
using MediatR;
using Stock.MediatR.Contracts.Requests;

namespace Stock.Infrastructure.Providers.MassTransit.Consumers.ReleaseProduct;

public class StockReleaseProductConsumer : IConsumer<StockReleaseProductCommand>
{
    private readonly IMediator _mediator;

    private readonly IPublishEndpoint _publishEndpoint;

    public StockReleaseProductConsumer(IMediator mediator,
        IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<StockReleaseProductCommand> context)
    {
        var request = context.Message;
        var mediatrRequest = new ReleaseProductRequest(request.ProductId,
            request.Quantity);
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        if (mediatrResponse.Success)
        {
            var stockReleasedEvent = new StockReleased(request.OrderId,
                request.ProductId,
                request.Quantity);
            await _publishEndpoint.Publish(stockReleasedEvent);
        }
    }
}