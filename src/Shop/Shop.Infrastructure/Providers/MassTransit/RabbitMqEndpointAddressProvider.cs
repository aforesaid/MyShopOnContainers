using MassTransit;

namespace Shop.Infrastructure.Providers.MassTransit;

public class RabbitMqEndpointAddressProvider :
    IEndpointAddressProvider
{
    readonly IEndpointNameFormatter _formatter;

    public RabbitMqEndpointAddressProvider(IEndpointNameFormatter formatter)
    {
        _formatter = formatter;
    }

    public Uri GetExecuteEndpoint<T, TArguments>()
        where T : class, IExecuteActivity<TArguments>
        where TArguments : class
    {
        return new Uri($"exchange:{_formatter.ExecuteActivity<T, TArguments>()}");
    }

    public Uri GetConsumerEndpoint<T, TArguments>()
        where T : class, IConsumer<TArguments>
        where TArguments : class
    {
        return new Uri($"queue:{_formatter.Consumer<T>()}");
    }
}