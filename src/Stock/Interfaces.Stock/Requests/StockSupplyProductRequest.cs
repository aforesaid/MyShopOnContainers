namespace Interfaces.Stock.Requests;

public class StockSupplyProductRequest
{
    public StockSupplyProductRequest()
    { }
    
    public StockSupplyProductRequest(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
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