namespace Stock.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    
    public DateTime Created { get; protected set; } = DateTime.UtcNow;
    public DateTime Updated { get; protected set; } = DateTime.UtcNow;

    protected void SetUpdated()
    {
        Updated = DateTime.UtcNow;
    }
}