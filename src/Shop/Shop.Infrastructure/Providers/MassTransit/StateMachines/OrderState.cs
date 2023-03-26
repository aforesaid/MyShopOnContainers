using MassTransit;

namespace Shop.Infrastructure.Providers.MassTransit.StateMachines;

public class OrderState
    : SagaStateMachineInstance
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public DateTime Updated { get; set; }
}