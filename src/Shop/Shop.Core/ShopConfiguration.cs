using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shop.Core;

public static class ShopConfiguration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return serviceCollection;
    }
}