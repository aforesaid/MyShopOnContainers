namespace Interfaces.Stock.Events;

public class StockNewSupply
{
    public StockNewSupply()
    { }

    public StockNewSupply(Guid productId, 
        int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}