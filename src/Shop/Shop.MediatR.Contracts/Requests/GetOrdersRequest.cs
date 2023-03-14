using MediatR;
using Shop.MediatR.Contracts.Shared;

namespace Shop.MediatR.Contracts.Requests;

public class GetOrdersRequest : IRequest<GetOrdersResponse>
{
    public GetOrdersRequest()
    { }

    public GetOrdersRequest(IEnumerable<Guid> userIds = null)
    {
        UserIds = userIds;
    }
    public IEnumerable<Guid> UserIds { get; set; }
}

public class GetOrdersResponse
{
    public GetOrdersResponse()
    { }

    public GetOrdersResponse(IEnumerable<OrderInfo> orders)
    {
        Orders = orders;
    }
    public IEnumerable<OrderInfo> Orders { get; set; }
}