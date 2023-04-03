using Shop.Domain.Enums;

namespace Shop.Domain.Entities;

public class OrderEntity : BaseEntity
{
    private OrderEntity()
    { }

    public OrderEntity(Guid userId, Guid productId, int quantity)
    {
        UserId = userId;
        ProductId = productId;
        Quantity = quantity < 1 ? throw new ArgumentException("Quantity can't be less then 1") : quantity;
        
        State = OrderStatesEnum.Created;
    }
    
    public Guid UserId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public OrderStatesEnum State { get; private set; }

    public void SetOrderState(OrderStatesEnum state)
    {
        State = state;
        SetUpdated();
    }
}