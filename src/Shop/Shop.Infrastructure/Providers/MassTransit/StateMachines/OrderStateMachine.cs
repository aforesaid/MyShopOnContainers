using Interfaces.Shop.Events;
using Interfaces.Stock.Events;
using Interfaces.Stock.Messages;
using MassTransit;

namespace Shop.Infrastructure.Providers.MassTransit.StateMachines;

public class OrderStateMachine  
    : MassTransitStateMachine<OrderState>

{
    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderCreated,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => OrderAccepted,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockReserved,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => OutOfStock,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockReleased,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => OrderCompleted,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => OrderCancelled,
            x => x.CorrelateById(m => m.Message.OrderId));
            
        Initially(
            When(OrderCreated)
                .Then(context =>
                {
                    context.Saga.Updated = DateTime.UtcNow;
                })  
                .TransitionTo(Created));
    }
    
    public State Created { get; set; }
    public State Accepted { get; set; }
    public State Reserved { get; set; }
    public State Completed { get; set; }
    
    public State Cancelled { get; set; }
    public State Faulted { get; set; }
    
    public Event<OrderCreated> OrderCreated { get; set; }
    public Event<OrderAccepted> OrderAccepted { get; set; }
    public Event<StockReserved> StockReserved { get; set; }
    public Event<StockOutOfStock> OutOfStock { get; set; }
    public Event<StockReleased> StockReleased { get; set; }
    public Event<OrderCompleted> OrderCompleted { get; set; }
    public Event<OrderCancelled> OrderCancelled { get; set; }
}