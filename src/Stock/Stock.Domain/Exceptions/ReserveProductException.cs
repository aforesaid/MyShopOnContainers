namespace Stock.Domain.Exceptions;

public class ReserveProductException : Exception
{
    public ReserveProductException()
    : base("Can't reserve product")
    { }
}