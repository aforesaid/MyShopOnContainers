﻿using MediatR;

namespace Stock.MediatR.Contracts.Requests;

public class AddProductRequest : IRequest<AddProductResponse>
{
    public AddProductRequest()
    { }

    public AddProductRequest(string productName, int available)
    {
        ProductName = productName;
        Available = available;
    }
    public string ProductName { get; set; }
    public int Available { get; set; }
}

public class AddProductResponse
{
    public AddProductResponse()
    { }
    public AddProductResponse(bool success)
    {
        Success = success;
    }
    public bool Success { get; set; }
}