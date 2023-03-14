using Shop.Domain.Enums;

namespace Shop.Domain.Entities;

public class OrderEntity : BaseEntity
{
    private OrderEntity()
    { }

    public OrderEntity(Guid userId, Guid productId, int productCount)
    {
        UserId = userId;
        ProductId = productId;
        ProductCount = productCount;
        
        State = OrderStatesEnum.Pending;
    }
    
    public Guid UserId { get; private set; }
    public Guid ProductId { get; private set; }
    public int ProductCount { get; private set; }
    public OrderStatesEnum State { get; private set; }

    public void SetOrderState(OrderStatesEnum state)
    {
        State = state;
        SetUpdated();
    }
}