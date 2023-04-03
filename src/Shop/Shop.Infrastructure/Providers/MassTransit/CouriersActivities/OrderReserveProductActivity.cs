using Interfaces.Stock.Commands;
using MassTransit;
using MediatR;
using Shop.MediatR.Contracts.Requests;

namespace Shop.Infrastructure.Providers.MassTransit.CouriersActivities;

public class OrderReserveProductActivity
: IExecuteActivity<OrderReserveProductActivityArguments>
{
    private readonly IMediator _mediator;
    private readonly IBusControl _busControl;

    public OrderReserveProductActivity(IMediator mediator,
        IBusControl busControl)
    {
        _mediator = mediator;
        _busControl = busControl;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<OrderReserveProductActivityArguments> context)
    {
        var mediatrRequest = new GetOrdersRequest(orderIds: new[] { context.Arguments.OrderId });
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        var orderInfo = mediatrResponse.Orders.First();

        var sendEndpoint = await _busControl.GetPublishSendEndpoint<StockReserveProductCommand>();
        
        var stockReserveProductCommand = new StockReserveProductCommand(orderInfo.OrderId,
            orderInfo.ProductId,
            orderInfo.Quantity);
        await sendEndpoint.Send(stockReserveProductCommand);

        return context.Completed();
    }
}

public class OrderAcceptActivityDefinition :
    ExecuteActivityDefinition<OrderReserveProductActivity, OrderReserveProductActivityArguments>
{
    public OrderAcceptActivityDefinition()
    {
        ConcurrentMessageLimit = 20;
    }
}