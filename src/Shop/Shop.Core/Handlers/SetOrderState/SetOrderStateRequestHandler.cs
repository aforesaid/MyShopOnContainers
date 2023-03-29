using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Database;
using Shop.MediatR.Contracts.Requests;

namespace Shop.Core.Handlers.SetOrderState;

public class SetOrderStateRequestHandler : IRequestHandler<SetOrderStateRequest, SetOrderStateResponse>
{
    private readonly ShopDbContext _shopDbContext;

    public SetOrderStateRequestHandler(ShopDbContext shopDbContext)
    {
        _shopDbContext = shopDbContext;
    }

    public async Task<SetOrderStateResponse> Handle(SetOrderStateRequest request, CancellationToken cancellationToken)
    {
        var orderInfo = await _shopDbContext.OrderEntities
            .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken: cancellationToken);

        if (orderInfo == null)
        {
            throw new ArgumentException($"Not found order {request.OrderId}");
        }
        
        orderInfo.SetOrderState(request.OrderState);

        _shopDbContext.Update(orderInfo);
        await _shopDbContext.SaveChangesAsync(cancellationToken);

        return new SetOrderStateResponse(success: true);
    }
}