using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;
using Shop.Infrastructure.Database.Configurations;

namespace Shop.Infrastructure.Database;

public class ShopDbContext : DbContext
{
    public DbSet<OrderEntity> OrderEntities { get; protected set; }
    
    public ShopDbContext() 
        : base(){ }
    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "User ID=postgres;Password=root;Server=localhost;Port=5432;Database=shop;Integrated Security=true;");
        }
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderEntityConfiguration).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}