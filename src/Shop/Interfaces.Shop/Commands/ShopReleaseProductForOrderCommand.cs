namespace Interfaces.Shop.Commands;

public class ShopReleaseProductForOrderCommand
{
    public ShopReleaseProductForOrderCommand()
    { }

    public ShopReleaseProductForOrderCommand(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
}