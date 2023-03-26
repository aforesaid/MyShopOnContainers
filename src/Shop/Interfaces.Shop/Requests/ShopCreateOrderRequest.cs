namespace Interfaces.Shop.Requests;

public class ShopCreateOrderRequest
{
    public ShopCreateOrderRequest()
    { }

    public ShopCreateOrderRequest(Guid userId,
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

public class ShopCreateOrderResponse
{
    public ShopCreateOrderResponse()
    { }
    
    public ShopCreateOrderResponse(Guid orderId)
    {
        OrderId = orderId;
    }
    
    public Guid OrderId { get; set; }
}