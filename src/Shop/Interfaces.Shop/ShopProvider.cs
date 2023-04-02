using Interfaces.Shop.Interfaces;
using Interfaces.Shop.Requests;
using MassTransit;

namespace Interfaces.Shop;

public class ShopProvider : IShopProvider
{
    private readonly IRequestClient<ShopGetOrdersRequest> _getOrdersClient;
    private readonly IRequestClient<ShopCreateOrderRequest> _createOrderClient;

    public ShopProvider(IRequestClient<ShopGetOrdersRequest> getOrdersClient, 
        IRequestClient<ShopCreateOrderRequest> createOrderClient)
    {
        _getOrdersClient = getOrdersClient;
        _createOrderClient = createOrderClient;
    }

    public async Task<ShopGetOrdersResponse> GetOrders(ShopGetOrdersRequest request, TimeSpan? timeOut = null)
    {
        var response = await _getOrdersClient.GetResponse<ShopGetOrdersResponse>(request, 
            timeout: timeOut ?? RequestTimeout.Default);
        return response.Message;
    }

    public async Task<ShopCreateOrderResponse> CreateOrder(ShopCreateOrderRequest request, TimeSpan? timeOut = null)
    {
        var response = await _createOrderClient.GetResponse<ShopCreateOrderResponse>(request, 
            timeout: timeOut ?? RequestTimeout.Default);
        return response.Message;
    }
}