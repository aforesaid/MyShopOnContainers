using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock.Infrastructure.Database;
using Stock.MediatR.Contracts.Requests;

namespace Stock.Core.Handlers.ReleaseProduct;

public class ReleaseProductRequestHandler : IRequestHandler<ReleaseProductRequest, ReleaseProductResponse>
{
    private readonly StockDbContext _stockDbContext;

    public ReleaseProductRequestHandler(StockDbContext stockDbContext)
    {
        _stockDbContext = stockDbContext;
    }

    public async Task<ReleaseProductResponse> Handle(ReleaseProductRequest request, CancellationToken cancellationToken)
    {
        var productEntity = await _stockDbContext.ProductEntities
            .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken: cancellationToken);
        if (productEntity == null)
        {
            throw new ArgumentException($"Not found product {request.ProductId}");
        }
        
        productEntity.Release(request.Quantity);
        _stockDbContext.Update(productEntity);
        await _stockDbContext.SaveChangesAsync(cancellationToken);

        return new ReleaseProductResponse(success: true);
    }
}