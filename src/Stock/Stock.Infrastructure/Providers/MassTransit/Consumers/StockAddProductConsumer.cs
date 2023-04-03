using Interfaces.Stock.Requests;
using MassTransit;
using MediatR;
using Stock.MediatR.Contracts.Requests;

namespace Stock.Infrastructure.Providers.MassTransit.Consumers;

public class StockAddProductConsumer : IConsumer<StockAddProductRequest>
{
    private readonly IMediator _mediator;

    public StockAddProductConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<StockAddProductRequest> context)
    {
        var request = context.Message;

        var mediatrRequest = new AddProductRequest(request.ProductName,
            request.Available);
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        var response = new StockAddProductResponse(mediatrResponse.Success);
        await context.RespondAsync(response);
    }
}