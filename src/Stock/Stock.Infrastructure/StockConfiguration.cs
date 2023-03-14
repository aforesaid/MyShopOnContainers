using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Infrastructure.Database;
using Stock.Infrastructure.Providers.MassTransit.Consumers.GetProductList;
using Stock.Infrastructure.Providers.MassTransit.Consumers.SupplyProduct;

namespace Stock.Infrastructure;

public static class StockConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        const string stockDbContextConnectionString = "POSTGRES";
        
        serviceCollection.AddDbContext<StockDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString(stockDbContextConnectionString)));
        
        AddMassTransit(serviceCollection, configuration);
        
        return serviceCollection;
    }

    private static IServiceCollection AddMassTransit(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddMassTransit(x =>
        {
            x.AddConsumer<StockGetProductListConsumer>(typeof(StockGetProductListConsumerDefinition));
            x.AddConsumer<StockSupplyProductConsumer>(typeof(StockSupplyProductConsumerDefinition));
            
            x.UsingRabbitMq((context, cfg) =>
            {
                const string prefetchCountHeader = "PrefetchCount";

                var prefetchCount = int.Parse(configuration[prefetchCountHeader] ?? string.Empty);
                cfg.PrefetchCount = prefetchCount;

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