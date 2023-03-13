using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Infrastructure.Database;

namespace Stock.Infrastructure;

public static class StockConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        const string stockDbContextConnectionString = "POSTGRES";
        
        serviceCollection.AddDbContext<StockDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString(stockDbContextConnectionString)));
        
        return serviceCollection;
    }
}