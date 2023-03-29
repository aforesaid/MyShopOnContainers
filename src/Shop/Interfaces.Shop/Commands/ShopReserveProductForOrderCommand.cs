namespace Interfaces.Shop.Commands;

public class ShopReserveProductForOrderCommand
{
    public ShopReserveProductForOrderCommand()
    { }

    public ShopReserveProductForOrderCommand(Guid orderId)
    {
        OrderId = orderId;
    }
    public Guid OrderId { get; set; }
}