﻿using MediatR;
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

        var orders = await q.AsNoTracking()
            .Select(x => new OrderInfo(x.UserId,
                x.Id,
                x.ProductId,
                x.ProductCount,
                x.State,
                x.Created,
                x.Updated))
            .ToListAsync(cancellationToken: cancellationToken);
        
        var result = new GetOrdersResponse(orders);
        return result;
    }
}