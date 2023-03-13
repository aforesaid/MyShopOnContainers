using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stock.Core;

public static class StockConfiguration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        
        return serviceCollection;
    }
}