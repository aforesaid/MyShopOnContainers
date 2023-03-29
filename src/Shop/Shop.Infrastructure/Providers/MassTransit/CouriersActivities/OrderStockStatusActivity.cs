using Interfaces.Stock.Interfaces;
using Interfaces.Stock.Requests;
using MassTransit;
using MediatR;
using Shop.MediatR.Contracts.Requests;

namespace Shop.Infrastructure.Providers.MassTransit.CouriersActivities;

public class OrderStockStatusActivity
: IExecuteActivity<OrderStockStatusActivityArguments>

{
    public static readonly Uri ExecuteAddress = new("queue:order-stock-status_execute");

    private readonly IMediator _mediator;
    private readonly IStockProvider _stockProvider;

    public OrderStockStatusActivity(IMediator mediator, IStockProvider stockProvider)
    {
        _mediator = mediator;
        _stockProvider = stockProvider;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<OrderStockStatusActivityArguments> context)
    {
        var mediatrRequest = new GetOrdersRequest(orderIds: new[] { context.Arguments.OrderId });
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        var orderInfo = mediatrResponse.Orders.First();

        var targetProductId = orderInfo.ProductId;

        var getStockProductInfoRequest = new StockGetProductListRequest(new[] { targetProductId });
        var getStockProductInfoResponse = await _stockProvider.GetProductList(getStockProductInfoRequest);

        var stockProductInfo = getStockProductInfoResponse.Products.First();

        if (stockProductInfo.Free < orderInfo.Quantity)
        {
            throw new ArgumentException("Can't reserve products");
        }
        
        return context.Completed();
    }
}