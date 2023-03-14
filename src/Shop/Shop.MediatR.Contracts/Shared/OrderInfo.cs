using Shop.Domain.Enums;

namespace Shop.MediatR.Contracts.Shared;

public class OrderInfo
{
    public OrderInfo()
    { }

    public OrderInfo(Guid userId, 
        Guid orderId,
        Guid productId, 
        int productCount,
        OrderStatesEnum orderState, 
        DateTime created,
        DateTime updated)
    {
        UserId = userId;
        OrderId = orderId;
        ProductId = productId;
        ProductCount = productCount;
        OrderState = orderState;
        Created = created;
        Updated = updated;
    }
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    
    public Guid ProductId { get; set; }
    public int ProductCount { get; set; }
    
    public OrderStatesEnum OrderState { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}