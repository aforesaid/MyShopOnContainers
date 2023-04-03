using Interfaces.Shop.Events;
using Interfaces.Shop.Interfaces;
using Interfaces.Shop.Requests;
using MassTransit;

namespace Interfaces.Shop;

public class ShopProvider : IShopProvider
{
    private readonly IBusControl _busControl;
    
    private readonly IRequestClient<ShopGetOrdersRequest> _getOrdersClient;
    private readonly IRequestClient<ShopCreateOrderRequest> _createOrderClient;

    public ShopProvider(IBusControl busControl,
        IRequestClient<ShopGetOrdersRequest> getOrdersClient, 
        IRequestClient<ShopCreateOrderRequest> createOrderClient)
    {
        _getOrdersClient = getOrdersClient;
        _createOrderClient = createOrderClient;
        _busControl = busControl;
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

    public async Task ReleaseOrder(Guid orderId)
    {
        var sendEvent = new OrderReleaseStockStarted(orderId: orderId);
        var sendEndpoint = await _busControl.GetPublishSendEndpoint<OrderReleaseStockStarted>();
        await sendEndpoint.Send(sendEvent);
    }

    public async Task CancelOrder(Guid orderId)
    {
        var sendEvent = new OrderCancellationStarted(orderId: orderId);
        var sendEndpoint = await _busControl.GetPublishSendEndpoint<OrderCancellationStarted>();
        await sendEndpoint.Send(sendEvent);
    }
}