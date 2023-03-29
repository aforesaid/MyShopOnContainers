using MediatR;

namespace Stock.MediatR.Contracts.Requests;

public class CancelReserveProductRequest : IRequest<CancelReserveProductResponse>
{
    public CancelReserveProductRequest()
    { }

    public CancelReserveProductRequest(Guid productId,
        int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CancelReserveProductResponse
{
    public CancelReserveProductResponse(bool success)
    {
        Success = success;
    }
    public bool Success { get; set; }
}