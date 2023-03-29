using MassTransit;
using MassTransit.RedisSagas;

namespace Shop.Infrastructure.Providers.MassTransit.StateMachines;

public class OrderState
    : SagaStateMachineInstance,
        IVersionedSaga
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public DateTime Updated { get; set; }
    public int Version { get; set; }
}