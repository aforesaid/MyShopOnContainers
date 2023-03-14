namespace Interfaces.Shop.Messages;

public class OrderCreated
{
    public OrderCreated()
    { }

    public OrderCreated(Guid userId,
        Guid orderId, 
        Guid productId,
        int productCount)
    {
        UserId = userId;
        OrderId = orderId;
        ProductId = productId;
        ProductCount = productCount;
    }
    
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    
    public Guid ProductId { get; set; }
    public int ProductCount { get; set; }
}