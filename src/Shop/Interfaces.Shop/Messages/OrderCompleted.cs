namespace Interfaces.Shop.Messages;

public class OrderCompleted
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}