namespace Interfaces.Shop.Events;

public class OrderCreated
{
    public OrderCreated()
    { }

    public OrderCreated(Guid userId,
        Guid orderId, 
        Guid productId,
        int quantity)
    {
        UserId = userId;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
    }
    
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}