using Interfaces.Stock.Interfaces;
using Interfaces.Stock.Requests;
using MassTransit;

namespace Interfaces.Stock;

public class StockProvider : IStockProvider
{
    private readonly IRequestClient<StockGetProductListRequest> _getProductListClient;
    private readonly IRequestClient<StockSupplyProductRequest> _supplyProductClient;
    private readonly IRequestClient<StockAddProductRequest> _addProductClient;

    public StockProvider(IRequestClient<StockGetProductListRequest> getProductListClient, 
        IRequestClient<StockSupplyProductRequest> supplyProductClient, 
        IRequestClient<StockAddProductRequest> addProductClient)
    {
        _getProductListClient = getProductListClient;
        _supplyProductClient = supplyProductClient;
        _addProductClient = addProductClient;
    }

    public async Task<StockGetProductListResponse> GetProductList(StockGetProductListRequest request, TimeSpan? timeOut = null)
    {
        var response = await _getProductListClient.GetResponse<StockGetProductListResponse>(request, 
            timeout: timeOut ?? RequestTimeout.Default);
        return response.Message;
    }

    public async Task<StockSupplyProductResponse> SupplyProduct(StockSupplyProductRequest request, TimeSpan? timeOut = null)
    {
        var response = await _supplyProductClient.GetResponse<StockSupplyProductResponse>(request, 
            timeout: timeOut ?? RequestTimeout.Default);
        return response.Message;
    }

    public async Task<StockAddProductResponse> AddProduct(StockAddProductRequest request, TimeSpan? timeOut = null)
    {
        var response = await _addProductClient.GetResponse<StockAddProductResponse>(request, 
            timeout: timeOut ?? RequestTimeout.Default);
        return response.Message;
    }
}