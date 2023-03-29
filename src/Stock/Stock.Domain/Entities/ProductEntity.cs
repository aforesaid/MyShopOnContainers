using System.ComponentModel.DataAnnotations;
using Stock.Domain.Exceptions;

namespace Stock.Domain.Entities;

public class ProductEntity : BaseEntity
{
    private const int MaxNameLength = 128;
    
    private ProductEntity()
    { }

    public ProductEntity(string name,
        int available = 0)
    {
        Name = name;
        Available = available;
    }

    [StringLength(MaxNameLength)]
    public string Name { get; private set; }
    public int Available { get; protected set; }
    public int Reserved { get; protected set; }

    public void Supply(int newCount)
    {
        Available += newCount;
        
        SetUpdated();
    }

    public void Reservation(int reservedCount)
    {
        if (Available >= reservedCount)
        {
            Available -= reservedCount;
            Reserved += reservedCount;
            
            SetUpdated();
        }
        else
        {
            throw new ReserveProductException();
        }
    }
    
    public void CancelReservation(int cancelReserveCount)
    {
        if (Reserved >= cancelReserveCount)
        {
            Reserved -= cancelReserveCount;
            Available += cancelReserveCount;

            SetUpdated();
        }
        else
        {
            throw new OutOfStockException();
        }
    }

    public void Release(int releaseCount)
    {
        if (Reserved >= releaseCount)
        {
            Reserved -= releaseCount;
            SetUpdated();
        }
        else
        {
            throw new ArgumentException();
        }
    }
}