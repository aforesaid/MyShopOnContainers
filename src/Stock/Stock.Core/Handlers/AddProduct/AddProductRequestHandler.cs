using MediatR;
using Stock.Domain.Entities;
using Stock.Infrastructure.Database;
using Stock.MediatR.Contracts.Requests;

namespace Stock.Core.Handlers.AddProduct;

public class AddProductRequestHandler : IRequestHandler<AddProductRequest, AddProductResponse>
{
    private readonly StockDbContext _stockDbContext;

    public AddProductRequestHandler(StockDbContext stockDbContext)
    {
        _stockDbContext = stockDbContext;
    }

    public async Task<AddProductResponse> Handle(AddProductRequest request, CancellationToken cancellationToken)
    {
        var productEntity = new ProductEntity(request.ProductName,
            request.Available);
        
        await _stockDbContext.AddAsync(productEntity, cancellationToken);
        await _stockDbContext.SaveChangesAsync(cancellationToken);

        return new AddProductResponse(productId: productEntity.Id);
    }
}