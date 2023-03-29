using MediatR;
using Shop.Domain.Enums;

namespace Shop.MediatR.Contracts.Requests;

public class SetOrderStateRequest : IRequest<SetOrderStateResponse>
{
    public SetOrderStateRequest()
    { }

    public SetOrderStateRequest(Guid orderId, OrderStatesEnum orderState)
    {
        OrderId = orderId;
        OrderState = orderState;
    }
    public Guid OrderId { get; set; }
    public OrderStatesEnum OrderState { get; set; }
}

public class SetOrderStateResponse
{
    public SetOrderStateResponse()
    { }

    public SetOrderStateResponse(bool success)
    {
        Success = success;
    }
    public bool Success { get; set; }
}