namespace Interfaces.Shop.Messages;

public class OrderRejected
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}