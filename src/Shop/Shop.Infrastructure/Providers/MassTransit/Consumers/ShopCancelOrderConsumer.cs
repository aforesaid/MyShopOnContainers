using Interfaces.Shop.Commands;
using MassTransit;

namespace Shop.Infrastructure.Providers.MassTransit.Consumers;

public class ShopCancelOrderConsumer : IConsumer<ShopCancelOrderCommand>
{
    public Task Consume(ConsumeContext<ShopCancelOrderCommand> context)
    {
        throw new NotImplementedException();
    }
}