using Interfaces.Shop;
using Interfaces.Shop.Interfaces;
using Interfaces.Stock;
using Interfaces.Stock.Interfaces;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Infrastructure;

public static class WebConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddScoped<IStockProvider, StockProvider>();
        serviceCollection.AddScoped<IShopProvider, ShopProvider>();
        
        serviceCollection.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RABBIT_HOST"], "/", h =>
                {
                    h.Username(configuration["RABBIT_LOGIN"]);
                    h.Password(configuration["RABBIT_PASSWORD"]);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return serviceCollection;
    }
}