using Interfaces.Stock.Requests;

namespace Interfaces.Stock.Interfaces;

public interface IStockProvider
{
    Task<StockGetProductListResponse> GetProductList(StockGetProductListRequest request, TimeSpan? timeOut = null);
    Task<StockSupplyProductResponse> SupplyProduct(StockSupplyProductRequest request, TimeSpan? timeOut = null);
}