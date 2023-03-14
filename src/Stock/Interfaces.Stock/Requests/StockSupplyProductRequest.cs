namespace Interfaces.Stock.Requests;

public class StockSupplyProductRequest
{
    public StockSupplyProductRequest()
    { }
    
    public StockSupplyProductRequest(Guid productId, int productCount)
    {
        ProductId = productId;
        ProductCount = productCount;
    }
    public Guid ProductId { get; set; }
    public int ProductCount { get; set; }
}

public class StockSupplyProductResponse
{
    public StockSupplyProductResponse()
    { }

    public StockSupplyProductResponse(bool success)
    {
        Success = success;
    }
    public bool Success { get; set; }
}