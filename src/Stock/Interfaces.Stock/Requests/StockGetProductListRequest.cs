using Interfaces.Stock.Shared;

namespace Interfaces.Stock.Requests;

public class StockGetProductListRequest
{
    public StockGetProductListRequest()
    { }

    public StockGetProductListRequest(IEnumerable<Guid> productIds = null)
    {
        ProductIds = productIds;
    }
    public IEnumerable<Guid> ProductIds { get; set; }
}

public class StockGetProductListResponse
{
    public StockGetProductListResponse()
    { }

    public StockGetProductListResponse(IEnumerable<StockProductInfo> products)
    {
        Products = products;
    }
    public IEnumerable<StockProductInfo> Products { get; set; }
}