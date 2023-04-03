using Interfaces.Stock.Interfaces;
using Interfaces.Stock.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

[Route("api/[controller]/[action]")]
public class StockController : ControllerBase
{
    private readonly ILogger<StockController> _logger;
    private readonly IStockProvider _stockProvider;

    public StockController(ILogger<StockController> logger, 
        IStockProvider stockProvider)
    {
        _logger = logger;
        _stockProvider = stockProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var getProductListRequest = new StockGetProductListRequest();
            var getProductListResponse = await _stockProvider.GetProductList(getProductListRequest);
            
            return Ok(getProductListResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't get stocks");
            return BadRequest(e);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] StockAddProductRequest request)
    {
        try
        {
            var addProductResponse = await _stockProvider.AddProduct(request);
            return Ok(addProductResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't add product");
            return BadRequest(e);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSupply([FromBody] StockSupplyProductRequest request)
    {
        try
        {
            var supplyProductResponse = await _stockProvider.SupplyProduct(request);
            return Ok(supplyProductResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't supply product");
            return BadRequest(e);
        }
    }
}