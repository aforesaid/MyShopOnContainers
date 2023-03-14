using MediatR;
using Stock.MediatR.Contracts.Shared;

namespace Stock.MediatR.Contracts.Requests.GetProductList;

public class GetProductListRequest : IRequest<GetProductListResponse>
{
    public GetProductListRequest()
    { }
    
    public GetProductListRequest(IEnumerable<Guid> productIds = null)
    {
        ProductIds = productIds;
    }
    public IEnumerable<Guid> ProductIds { get; set; }
}

public class GetProductListResponse
{
    public GetProductListResponse()
    { }

    public GetProductListResponse(IEnumerable<ProductInfo> products)
    {
        Products = products;
    }
    public IEnumerable<ProductInfo> Products { get; set; }
}