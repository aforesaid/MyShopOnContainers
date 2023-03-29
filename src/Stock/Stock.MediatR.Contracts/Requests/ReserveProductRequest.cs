using MediatR;

namespace Stock.MediatR.Contracts.Requests;

public class ReserveProductRequest : IRequest<ReserveProductResponse>
{
    public ReserveProductRequest()
    { }
    
    public ReserveProductRequest(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class ReserveProductResponse
{
    public ReserveProductResponse()
    { }
    
    public ReserveProductResponse(bool success)
    {
        Success = success;
    }
    public bool Success { get; set; }
}