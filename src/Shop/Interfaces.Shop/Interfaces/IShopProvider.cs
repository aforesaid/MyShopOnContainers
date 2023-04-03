using Interfaces.Shop.Requests;

namespace Interfaces.Shop.Interfaces;

public interface IShopProvider
{
    Task<ShopGetOrdersResponse> GetOrders(ShopGetOrdersRequest request, TimeSpan? timeOut = null);
    Task<ShopCreateOrderResponse> CreateOrder(ShopCreateOrderRequest request, TimeSpan? timeOut = null);
    Task ReleaseOrder(Guid orderId);
    Task CancelOrder(Guid orderId);
}