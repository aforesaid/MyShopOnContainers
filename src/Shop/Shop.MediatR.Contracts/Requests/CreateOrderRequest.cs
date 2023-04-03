using MediatR;

namespace Shop.MediatR.Contracts.Requests;

public class CreateOrderRequest : IRequest<CreateOrderResponse>
{
    public CreateOrderRequest()
    { }

    public CreateOrderRequest(
        Guid userId,
        Guid productId, 
        int quantity)
    {
        UserId = userId;
        ProductId = productId;
        Quantity = quantity;
    }
    public Guid UserId { get; set; }
    
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateOrderResponse
{
    public CreateOrderResponse()
    { }

    public CreateOrderResponse(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
}