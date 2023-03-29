using MediatR;
using Shop.Domain.Entities;
using Shop.Infrastructure.Database;
using Shop.MediatR.Contracts.Requests;

namespace Shop.Core.Handlers.CreateOrder;

public class CreateOrderRequestHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
{
    private readonly ShopDbContext _shopDbContext;

    public CreateOrderRequestHandler(ShopDbContext shopDbContext)
    {
        _shopDbContext = shopDbContext;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var newOrderEntity = new OrderEntity(request.UserId,
            request.ProductId,
            request.Quantity);
        
        await _shopDbContext.AddAsync(newOrderEntity, cancellationToken);
        await _shopDbContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderResponse(newOrderEntity.Id);
    }
}