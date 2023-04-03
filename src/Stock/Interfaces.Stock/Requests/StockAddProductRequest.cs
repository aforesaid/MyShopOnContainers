namespace Interfaces.Stock.Requests;

public class StockAddProductRequest
{
    public StockAddProductRequest()
    { }

    public StockAddProductRequest(string productName, 
        int available = 0)
    {
        ProductName = productName;
        Available = available;
    }
    public string ProductName { get; set; }
    public int Available { get; set; }
}

public class StockAddProductResponse
{
    public StockAddProductResponse()
    { }

    public StockAddProductResponse(Guid productId)
    {
        ProductId = productId;
    }
    public Guid ProductId { get; set; }
}