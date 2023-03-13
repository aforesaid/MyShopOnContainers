using Microsoft.EntityFrameworkCore;

namespace Stock.Infrastructure.Database;

public class StockDbContext : DbContext
{
    public StockDbContext() { }
    
    public StockDbContext(DbContextOptions<StockDbContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "User ID=postgres;Password=root;Server=localhost;Port=5432;Database=stock;Integrated Security=true;");
        }
        base.OnConfiguring(optionsBuilder);
    }
}