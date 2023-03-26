using MediatR;

namespace Stock.MediatR.Contracts.Commands.SupplyProduct;

public class SupplyProductCommand : IRequest<bool>
{
    public SupplyProductCommand()
    { }

    public SupplyProductCommand(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity > 0 ? quantity : throw new ArgumentException($"Product count can't be {quantity}");
    }
    
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}