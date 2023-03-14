﻿using Interfaces.Shop.Requests;

namespace Interfaces.Shop.Interfaces;

public interface IShopProvider
{
    Task<ShopGetOrdersResponse> GetOrders(ShopGetOrdersRequest request, TimeSpan? timeOut = null);
}