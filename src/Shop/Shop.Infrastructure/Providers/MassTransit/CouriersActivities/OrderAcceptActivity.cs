using Interfaces.Stock.Commands;
using MassTransit;
using MediatR;
using Shop.MediatR.Contracts.Requests;

namespace Shop.Infrastructure.Providers.MassTransit.CouriersActivities;

public class OrderAcceptActivity
: IExecuteActivity<OrderAcceptActivityArguments>
{
    private readonly IMediator _mediator;
    private readonly ISendEndpoint _sendEndpoint;

    public OrderAcceptActivity(IMediator mediator,
        ISendEndpoint sendEndpoint)
    {
        _mediator = mediator;
        _sendEndpoint = sendEndpoint;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<OrderAcceptActivityArguments> context)
    {
        var mediatrRequest = new GetOrdersRequest(orderIds: new[] { context.Arguments.OrderId });
        var mediatrResponse = await _mediator.Send(mediatrRequest);

        var orderInfo = mediatrResponse.Orders.First();
        var stockReserveProductCommand = new StockReserveProductCommand(orderInfo.OrderId,
            orderInfo.ProductId,
            orderInfo.Quantity);
        await _sendEndpoint.Send(stockReserveProductCommand);

        return context.Completed();
    }
}

public class OrderAcceptActivityDefinition :
    ExecuteActivityDefinition<OrderAcceptActivity, OrderAcceptActivityArguments>
{
    public OrderAcceptActivityDefinition()
    {
        ConcurrentMessageLimit = 20;
    }
}