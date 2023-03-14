namespace Shop.Domain.Enums;

public enum OrderStatesEnum
{
    None = 0,
    Pending = 10,
    Reserved = 20,
    Completed = 30,
    Cancelled = 100
}