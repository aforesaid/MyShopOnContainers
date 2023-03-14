using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Infrastructure.Database;

namespace Shop.Infrastructure;

public static class ShopConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        const string shopDbContextConnectionString = "POSTGRES";

        serviceCollection.AddDbContext<ShopDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString(shopDbContextConnectionString)));
        
        return serviceCollection;
    }
}