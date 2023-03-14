using Interfaces.Stock.Requests;
using Interfaces.Stock.Shared;
using MassTransit;
using MediatR;
using Stock.MediatR.Contracts.Requests.GetProductList;

namespace Stock.Infrastructure.Providers.MassTransit.Consumers.GetProductList;

public class StockGetProductListConsumer : IConsumer<StockGetProductListRequest>
{
    private readonly IMediator _mediator;

    public StockGetProductListConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<StockGetProductListRequest> context)
    {
        var request = context.Message;

        var mediatrRequest = new GetProductListRequest(request.ProductIds);
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        var response = new StockGetProductListResponse(mediatrResponse.Products
            .Select(x => new StockProductInfo(x.ProductId,
                x.ProductName,
                x.Free)));
        
        await context.RespondAsync<StockGetProductListResponse>(response);
    }
}

public class StockGetProductListConsumerDefinition : ConsumerDefinition<StockGetProductListConsumer>
{
    private const int ConcurrencyLimit = 15;
    private static readonly TimeSpan TimeOut = TimeSpan.FromMinutes(5);

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<StockGetProductListConsumer> consumerConfigurator)
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