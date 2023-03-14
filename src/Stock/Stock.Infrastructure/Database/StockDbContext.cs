using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Infrastructure.Database.Configurations;

namespace Stock.Infrastructure.Database;

public class StockDbContext : DbContext
{
    public DbSet<ProductEntity> ProductEntities { get; protected set; }
    
    public StockDbContext() 
        : base(){ }
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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GoodEntityConfiguration).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}