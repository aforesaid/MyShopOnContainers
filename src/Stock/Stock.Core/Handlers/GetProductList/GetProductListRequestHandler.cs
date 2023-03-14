using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock.Infrastructure.Database;
using Stock.MediatR.Contracts.Requests.GetProductList;
using Stock.MediatR.Contracts.Shared;

namespace Stock.Core.Handlers.GetProductList;

public class GetProductListRequestHandler : IRequestHandler<GetProductListRequest, GetProductListResponse>
{
    private readonly StockDbContext _stockDbContext;

    public GetProductListRequestHandler(StockDbContext stockDbContext)
    {
        _stockDbContext = stockDbContext;
    }

    public async Task<GetProductListResponse> Handle(GetProductListRequest request, CancellationToken cancellationToken)
    {
        var q = _stockDbContext.ProductEntities
            .AsQueryable();

        if (request.ProductIds != null && request.ProductIds.Any())
        {
            q = q.Where(x => request.ProductIds.Contains(x.Id));
        }

        var products = await q.AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);

        var resultItems = products.Select(x =>
        {
            var free = x.Available - x.Reserved;
            
            return new ProductInfo(x.Id,
                x.Name,
                free);
        });

        var result = new GetProductListResponse(resultItems);
        return result;
    }
}