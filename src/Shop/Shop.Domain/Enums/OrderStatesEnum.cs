namespace Shop.Domain.Enums;

public enum OrderStatesEnum
{
    None = 0,
    Created = 10,
    Accepted = 20,
    Reserved = 30,
    Completed = 40,
    Canceled = 90,
    Rejected = 100
}