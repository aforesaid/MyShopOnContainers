using MediatR;
using Microsoft.EntityFrameworkCore;
using Stock.Infrastructure.Database;
using Stock.MediatR.Contracts.Commands.SupplyProduct;

namespace Stock.Core.Handlers.SupplyProduct;

public class SupplyProductRequestHandler : IRequestHandler<SupplyProductCommand, bool>
{
    private readonly StockDbContext _stockDbContext;

    public SupplyProductRequestHandler(StockDbContext stockDbContext)
    {
        _stockDbContext = stockDbContext;
    }

    public async Task<bool> Handle(SupplyProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _stockDbContext.ProductEntities
            .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken: cancellationToken);
        if (product == null)
        {
            throw new ArgumentException($"Not found product with Id {request.ProductId}");
        }
        
        product.Supply(request.ProductCount);
        await _stockDbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}