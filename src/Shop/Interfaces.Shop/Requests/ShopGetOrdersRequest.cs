using Interfaces.Shop.Shared;

namespace Interfaces.Shop.Requests;

public class ShopGetOrdersRequest
{
    public ShopGetOrdersRequest()
    { }
    
    public ShopGetOrdersRequest(IEnumerable<Guid> userIds = null)
    {
        UserIds = userIds;
    }
    public IEnumerable<Guid> UserIds { get; set; }
}

public class ShopGetOrdersResponse
{
    public ShopGetOrdersResponse()
    { }
    
    public ShopGetOrdersResponse(IEnumerable<ShopOrderInfo> orders)
    {
        Orders = orders;
    }
    public IEnumerable<ShopOrderInfo> Orders { get; set; }
}