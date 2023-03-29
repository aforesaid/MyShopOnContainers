namespace Stock.Domain.Exceptions;

public class OutOfStockException : Exception
{
    public OutOfStockException() 
        : base("Not found available stocks")
    { }
}