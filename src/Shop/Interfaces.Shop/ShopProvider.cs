using Interfaces.Shop.Interfaces;
using Interfaces.Shop.Requests;
using MassTransit;

namespace Interfaces.Shop;

public class ShopProvider : IShopProvider
{
    private readonly IRequestClient<ShopGetOrdersRequest> _getOrdersClient;

    public ShopProvider(IRequestClient<ShopGetOrdersRequest> getOrdersClient)
    {
        _getOrdersClient = getOrdersClient;
    }

    public async Task<ShopGetOrdersResponse> GetOrders(ShopGetOrdersRequest request, TimeSpan? timeOut = null)
    {
        var response = await _getOrdersClient.GetResponse<ShopGetOrdersResponse>(request, 
            timeout: timeOut ?? RequestTimeout.Default);
        return response.Message;
    }

    public async Task<ShopCreateOrderResponse> CreateOrder(ShopCreateOrderRequest request, TimeSpan? timeOut = null)
    {
        var response = await _getOrdersClient.GetResponse<ShopCreateOrderResponse>(request, 
            timeout: timeOut ?? RequestTimeout.Default);
        return response.Message;
    }
}