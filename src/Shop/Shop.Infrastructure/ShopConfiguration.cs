using Interfaces.Stock;
using Interfaces.Stock.Interfaces;
using MassTransit;
using MassTransit.MongoDbIntegration.MessageData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Infrastructure.Database;
using Shop.Infrastructure.Providers.MassTransit;
using Shop.Infrastructure.Providers.MassTransit.StateMachines;

namespace Shop.Infrastructure;

public static class ShopConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        const string shopDbContextConnectionString = "POSTGRES";

        serviceCollection.AddDbContext<ShopDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString(shopDbContextConnectionString)));

        AddMassTransit(serviceCollection, configuration);
        
        return serviceCollection;
    }

    private static IServiceCollection AddMassTransit(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IStockProvider, StockProvider>();
        serviceCollection.AddScoped<IEndpointAddressProvider, RabbitMqEndpointAddressProvider>();
        
        serviceCollection.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            const string ordersDatabase = "orders";
            x.AddConsumers(typeof(ShopConfiguration).Assembly);
            x.AddActivities(typeof(ShopConfiguration).Assembly);

            x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                .MongoDbRepository(r =>
                {
                    r.Connection = configuration.GetConnectionString("MongoDb");
                    r.DatabaseName = ordersDatabase;
                });
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseMessageData(new MongoDbMessageDataRepository(configuration.GetConnectionString("MongoDb"), ordersDatabase));

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