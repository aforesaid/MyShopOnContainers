using MassTransit;

namespace Shop.Infrastructure.Providers.MassTransit;

public interface IEndpointAddressProvider
{
    Uri GetExecuteEndpoint<T, TArguments>()
        where T : class, IExecuteActivity<TArguments>
        where TArguments : class;
}