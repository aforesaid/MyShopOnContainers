using Interfaces.Shop.Commands;
using Interfaces.Shop.Events;
using Interfaces.Stock.Events;
using Interfaces.Stock.Messages;
using MassTransit;
using MediatR;
using Shop.Domain.Enums;
using Shop.MediatR.Contracts.Requests;

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
        Event(() => ReserveCancelled,
            x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => OrderFaulted,
            x => x.CorrelateById(m => m.Message.OrderId));
        
        Initially(
            When(OrderCreated)
                .ThenAsync(async context =>
                {
                    context.Saga.Updated = DateTime.UtcNow;

                    var mediator = context.GetServiceOrCreateInstance<IMediator>();
                    var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                        OrderStatesEnum.Created);
                    await mediator.Send(setOrderStateRequest);
                })
                .ThenAsync(async context =>
                {
                    var bus = context.GetServiceOrCreateInstance<IBus>();
                    var sendEndpoint = await bus.GetPublishSendEndpoint<ShopReserveProductForOrderCommand>();

                    var sendRequest = new ShopReserveProductForOrderCommand(context.Message.OrderId);
                    await sendEndpoint.Send(sendRequest);
                })
                .TransitionTo(Created));

        During(Created,
            When(OrderAccepted)
                .ThenAsync(async context =>
                {
                    var mediator = context.GetServiceOrCreateInstance<IMediator>();
                    var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                        OrderStatesEnum.Accepted);
                    await mediator.Send(setOrderStateRequest);
                })
                .TransitionTo(Accepted),
            When(OrderFaulted)
                .TransitionTo(Faulted));
        
        During(Accepted,
            When(StockReserved)
                .ThenAsync(async context =>
                {
                    var mediator = context.GetServiceOrCreateInstance<IMediator>();
                    var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                        OrderStatesEnum.Reserved);
                    await mediator.Send(setOrderStateRequest);
                })
                .TransitionTo(Reserved),
            When(OutOfStock)
                .ThenAsync(async context =>
                {
                    var mediator = context.GetServiceOrCreateInstance<IMediator>();
                    var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                        OrderStatesEnum.Rejected);
                    await mediator.Send(setOrderStateRequest);
                })
                .Send(context => new OrderFaulted(context.Message.OrderId))
                .TransitionTo(Faulted));
        
        During(Reserved,
            When(OrderFaulted)
                .TransitionTo(Faulted),
            When(ReserveCancelled)
                .ThenAsync(async context =>
                {
                    var mediator = context.GetServiceOrCreateInstance<IMediator>();
                    var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                        OrderStatesEnum.Canceled);
                    await mediator.Send(setOrderStateRequest);
                })
                .Send(context => new OrderCancelled(context.Message.OrderId))
                .TransitionTo(Canceled),
            When(StockReleased)
                .ThenAsync(async context =>
                {
                    var mediator = context.GetServiceOrCreateInstance<IMediator>();
                    var setOrderStateRequest = new SetOrderStateRequest(context.Message.OrderId,
                        OrderStatesEnum.Completed);
                    await mediator.Send(setOrderStateRequest);
                })
                .Send(context => new OrderCompleted(context.Message.OrderId))
                .TransitionTo(Completed));
    }
    
    public State Created { get; set; }
    public State Accepted { get; set; }
    public State Reserved { get; set; }
    public State Completed { get; set; }
    
    public State PreCanceled { get; set; }
    public State Canceled { get; set; }
    public State Faulted { get; set; }
    
    public Event<OrderCreated> OrderCreated { get; set; }
    public Event<OrderAccepted> OrderAccepted { get; set; }
    public Event<StockReserved> StockReserved { get; set; }
    public Event<StockOutOfStock> OutOfStock { get; set; }
    public Event<StockReserveCancelled> ReserveCancelled { get; set; }
    public Event<StockReleased> StockReleased { get; set; }
    public Event<OrderFaulted> OrderFaulted { get; set; }

}