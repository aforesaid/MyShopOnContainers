using MassTransit;
using MongoDB.Bson.Serialization.Attributes;

namespace Shop.Infrastructure.Providers.MassTransit.StateMachines;

public class OrderState
    : SagaStateMachineInstance, 
        ISagaVersion
{
    [BsonId]
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    
    public DateTime Updated { get; set; }
    public int Version { get; set; }
}