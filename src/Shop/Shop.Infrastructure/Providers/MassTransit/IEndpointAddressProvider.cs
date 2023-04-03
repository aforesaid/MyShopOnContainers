using MassTransit;

namespace Shop.Infrastructure.Providers.MassTransit;

public interface IEndpointAddressProvider
{
    Uri GetExecuteEndpoint<T, TArguments>()
        where T : class, IExecuteActivity<TArguments>
        where TArguments : class;
    Uri GetConsumerEndpoint<T, TArguments>()
        where T : class, IConsumer<TArguments>
        where TArguments : class;
}