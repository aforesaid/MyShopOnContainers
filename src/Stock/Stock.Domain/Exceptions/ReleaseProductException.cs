namespace Stock.Domain.Exceptions;

public class ReleaseProductException : Exception
{
    public ReleaseProductException()
        : base("Can't release products")
    { }
}