using Interfaces.Shop.Enums;

namespace Interfaces.Shop.Shared;

public class ShopOrderInfo
{
    public ShopOrderInfo()
    { }

    public ShopOrderInfo(Guid userId,
        Guid orderId, 
        Guid productId, 
        int productCount, 
        ShopOrderStatesEnum orderState, 
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
    
    public ShopOrderStatesEnum OrderState { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}