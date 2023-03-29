namespace Interfaces.Shop.Commands;

public class ShopCancelOrderCommand
{
    public ShopCancelOrderCommand()
    { }

    public ShopCancelOrderCommand(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
}