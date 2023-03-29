using MediatR;

namespace Stock.MediatR.Contracts.Requests;

public class ReleaseProductRequest : IRequest<ReleaseProductResponse>
{
    public ReleaseProductRequest()
    { }

    public ReleaseProductRequest(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class ReleaseProductResponse
{
    public ReleaseProductResponse()
    { }

    public ReleaseProductResponse(bool success)
    {
        Success = success;
    }
    public bool Success { get; set; }
}