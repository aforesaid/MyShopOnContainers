using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock.Infrastructure.Database;
using Stock.MediatR.Contracts.Requests;

namespace Stock.Core.Handlers.ReserveProduct;

public class ReserveProductRequestHandler : IRequestHandler<ReserveProductRequest, ReserveProductResponse>
{
    private readonly StockDbContext _stockDbContext;

    public ReserveProductRequestHandler(StockDbContext stockDbContext)
    {
        _stockDbContext = stockDbContext;
    }

    public async Task<ReserveProductResponse> Handle(ReserveProductRequest request, CancellationToken cancellationToken)
    {
        var productInfo = await _stockDbContext.ProductEntities
            .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken: cancellationToken);

        if (productInfo == null)
        {
            throw new ArgumentException($"ProductInfo {request.ProductId} not found");
        }
        
        productInfo.Reservation(request.Quantity);

        _stockDbContext.Update(productInfo);
        
        await _stockDbContext.SaveChangesAsync(cancellationToken);
        
        return new ReserveProductResponse(success: true);
    }
}