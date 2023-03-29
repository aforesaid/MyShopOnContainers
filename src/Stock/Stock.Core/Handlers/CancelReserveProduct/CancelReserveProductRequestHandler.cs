using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock.Infrastructure.Database;
using Stock.MediatR.Contracts.Requests;

namespace Stock.Core.Handlers.CancelReserveProduct;

public class CancelReserveProductRequestHandler : IRequestHandler<CancelReserveProductRequest, CancelReserveProductResponse>
{
    private readonly StockDbContext _stockDbContext;

    public CancelReserveProductRequestHandler(StockDbContext stockDbContext)
    {
        _stockDbContext = stockDbContext;
    }

    public async Task<CancelReserveProductResponse> Handle(CancelReserveProductRequest request, CancellationToken cancellationToken)
    {
        var productInfo = await _stockDbContext.ProductEntities
            .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken: cancellationToken);

        if (productInfo == null)
        {
            throw new ArgumentException($"Product not found {request.ProductId}");
        }
        productInfo.CancelReservation(request.Quantity);

        _stockDbContext.Update(productInfo);
        await _stockDbContext.SaveChangesAsync(cancellationToken);

        return new CancelReserveProductResponse(success: true);
    }
}