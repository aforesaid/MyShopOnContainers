namespace Stock.MediatR.Contracts.Shared;

public class ProductInfo
{
    public ProductInfo()
    { }

    public ProductInfo(Guid productId,
        string productName,
        int free)
    {
        ProductId = productId;
        ProductName = productName;
        Free = free;
    }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Free { get; set; }
}