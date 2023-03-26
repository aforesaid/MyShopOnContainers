using Interfaces.Shop.Enums;

namespace Interfaces.Shop.Shared;

public class ShopOrderInfo
{
    public ShopOrderInfo()
    { }

    public ShopOrderInfo(Guid userId,
        Guid orderId, 
        Guid productId, 
        int quantity, 
        ShopOrderStatesEnum orderState, 
        DateTime created,
        DateTime updated)
    {
        UserId = userId;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        OrderState = orderState;
        Created = created;
        Updated = updated;
    }
    
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    
    public ShopOrderStatesEnum OrderState { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}