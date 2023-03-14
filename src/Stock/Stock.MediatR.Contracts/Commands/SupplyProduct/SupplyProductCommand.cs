using MediatR;

namespace Stock.MediatR.Contracts.Commands.SupplyProduct;

public class SupplyProductCommand : IRequest<bool>
{
    public SupplyProductCommand()
    { }

    public SupplyProductCommand(Guid productId, int productCount)
    {
        ProductId = productId;
        ProductCount = productCount > 0 ? productCount : throw new ArgumentException($"Product count can't be {productCount}");
    }
    
    public Guid ProductId { get; set; }
    public int ProductCount { get; set; }
}