using Interfaces.Shop.Interfaces;
using Interfaces.Shop.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

[Route("api/[controller]/[action]")]
public class ShopController : ControllerBase
{
    private readonly ILogger<ShopController> _logger;
    private readonly IShopProvider _shopProvider;
    public ShopController(IShopProvider shopProvider, 
        ILogger<ShopController> logger)
    {
        _shopProvider = shopProvider;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] Guid userId)
    {
        try
        {
            var getOrdersRequest = new ShopGetOrdersRequest(userIds: new[] { userId });
            var getOrdersResponse = await _shopProvider.GetOrders(getOrdersRequest);

            return Ok(getOrdersResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't get orders for userId {userId}",
                userId);
            return BadRequest(e);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] ShopCreateOrderRequest requestModel)
    {
        try
        {
            var createOrderResponse = await _shopProvider.CreateOrder(requestModel);
            return Ok(createOrderResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't create order");
            return BadRequest(e);
        }
    }

    [HttpPut]
    public async Task<IActionResult> ReleaseOrder([FromQuery] Guid orderId)
    {
        try
        {
            await _shopProvider.ReleaseOrder(orderId);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't release order");
            return BadRequest(e);
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> CancelOrder([FromQuery] Guid orderId)
    {
        try
        {
            await _shopProvider.CancelOrder(orderId);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't release order");
            return BadRequest(e);
        }
    }
}