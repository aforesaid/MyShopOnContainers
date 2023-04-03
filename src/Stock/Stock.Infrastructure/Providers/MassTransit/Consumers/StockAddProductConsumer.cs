using Interfaces.Stock.Events;
using Interfaces.Stock.Requests;
using MassTransit;
using MediatR;
using Stock.MediatR.Contracts.Requests;

namespace Stock.Infrastructure.Providers.MassTransit.Consumers;

public class StockAddProductConsumer : IConsumer<StockAddProductRequest>
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;


    public StockAddProductConsumer(IMediator mediator, 
        IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<StockAddProductRequest> context)
    {
        var request = context.Message;

        var mediatrRequest = new AddProductRequest(request.ProductName,
            request.Available);
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        if (request.Available > 0)
        {
            var message = new StockNewSupply(mediatrResponse.ProductId,
                request.Available);
            await _publishEndpoint.Publish(message);
        }
        
        var response = new StockAddProductResponse(mediatrResponse.ProductId);
        await context.RespondAsync(response);
    }
}