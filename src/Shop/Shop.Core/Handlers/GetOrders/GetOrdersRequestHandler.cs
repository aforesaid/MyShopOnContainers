using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Database;
using Shop.MediatR.Contracts.Requests;
using Shop.MediatR.Contracts.Shared;

namespace Shop.Core.Handlers.GetOrders;

public class GetOrdersRequestHandler : IRequestHandler<GetOrdersRequest, GetOrdersResponse>
{
    private readonly ShopDbContext _shopDbContext;

    public GetOrdersRequestHandler(ShopDbContext shopDbContext)
    {
        _shopDbContext = shopDbContext;
    }

    public async Task<GetOrdersResponse> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
    {
        var q = _shopDbContext.OrderEntities
            .AsQueryable();

        if (request.UserIds != null && request.UserIds.Any())
        {
            q = q.Where(x => request.UserIds.Contains(x.UserId));
        }

        if (request.OrderIds != null && request.OrderIds.Any())
        {
            q = q.Where(x => request.OrderIds.Contains(x.Id));
        }
        
        var orders = await q.AsNoTracking()
            .OrderByDescending(x => x.Updated)
            .Select(x => new OrderInfo(x.UserId,
                x.Id,
                x.ProductId,
                x.Quantity,
                x.State,
                x.Created,
                x.Updated))
            .ToListAsync(cancellationToken: cancellationToken);
        
        var result = new GetOrdersResponse(orders);
        return result;
    }
}